
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Application.DTOs.ScheduleOverride;
using ScheduleApi.Application.Services;
using ScheduleApi.Core.Entities;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Schedule.Queries;

public static class GetGroupWeeklySchedule
{
    public record Query(int GroupId, string TimeZoneId, DateTime? TargetDate) : IRequest<WeeklyScheduleDto>;

    // Вспомогательный record для передачи сырых данных между методами
    private record WeeklyScheduleRawData(
        List<Core.Entities.ScheduleOverride> Overrides,
        List<ScheduleDataDto> AllPossibleLessons
    );
    
    private class Handler : IRequestHandler<Query, WeeklyScheduleDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;
        private readonly IScheduleTimeContextService _timeContextService;

        public Handler(ScheduleContext ctx, IMapper mapper, IScheduleTimeContextService timeContextService)
        {
            _ctx = ctx;
            _mapper = mapper;
            _timeContextService = timeContextService;
        }

        // 1. МЕТОД-ОРКЕСТРАТОР
        public async Task<WeeklyScheduleDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var group = await _ctx.Groups.AsNoTracking().FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken)
                ?? throw new NotFoundException($"Group with ID {request.GroupId} was not found.");

            var timeContext = _timeContextService.CreateContext(request.TargetDate, request.TimeZoneId);
            var authoritativeTimeZone = _timeContextService.GetAuthoritativeTimeZone();
            var (startOfWeek, endOfWeek) = GetAuthoritativeWeekBoundaries(timeContext, authoritativeTimeZone);

            var semester = await _ctx.Semesters.AsNoTracking()
                .FirstOrDefaultAsync(s => s.StartDate < endOfWeek.UtcDateTime && s.EndDate >= startOfWeek.UtcDateTime, cancellationToken);

            if (semester is null)
                return CreateEmptyWeeklySchedule(group, startOfWeek.DateTime);

            // Получаем все данные одним вызовом
            var rawData = await FetchWeeklyScheduleDataAsync(request, semester, startOfWeek.UtcDateTime, endOfWeek.UtcDateTime, cancellationToken);
            
            // Получаем дни недели один раз
            var daysOfWeek = await _ctx.ApplicationDaysOfWeek.AsNoTracking().OrderBy(d => d.Id).ToListAsync(cancellationToken);

            // Собираем расписание из сырых данных
            var dailySchedules = BuildDailySchedules(rawData, group, semester, startOfWeek.DateTime, daysOfWeek, authoritativeTimeZone);

            var firstDaySchedule = dailySchedules.First();

            return new WeeklyScheduleDto
            {
                GroupName = group.Name,
                WeekStartDate = DateOnly.FromDateTime(startOfWeek.DateTime),
                WeekEndDate = DateOnly.FromDateTime(startOfWeek.DateTime.AddDays(6)),
                WeekNumber = firstDaySchedule.WeekNumber,
                IsEvenWeek = firstDaySchedule.IsEvenWeek,
                TimeZoneId = request.TimeZoneId,
                DailySchedules = dailySchedules
            };
        }

        // 2. МЕТОД ДЛЯ ВЫЧИСЛЕНИЯ ГРАНИЦ НЕДЕЛИ
        /// <summary>
        /// Вычисляет начало (понедельник 00:00) и конец (следующий понедельник 00:00) недели в авторитетном часовом поясе.
        /// </summary>
        private (DateTimeOffset Start, DateTimeOffset End) GetAuthoritativeWeekBoundaries(ScheduleTimeContext timeContext, TimeZoneInfo authoritativeTimeZone)
        {
            var targetDay = timeContext.AuthoritativeTime;
            int diff = (7 + (int)targetDay.DayOfWeek - (int)DayOfWeek.Monday) % 7;
            var startOfWeek = targetDay.AddDays(-1 * diff).Date;
            var endOfWeek = startOfWeek.AddDays(7);

            return (
                new DateTimeOffset(startOfWeek, authoritativeTimeZone.GetUtcOffset(startOfWeek)),
                new DateTimeOffset(endOfWeek, authoritativeTimeZone.GetUtcOffset(endOfWeek))
            );
        }

        // 3. МЕТОД ДЛЯ ПОЛУЧЕНИЯ ДАННЫХ ИЗ БД
        /// <summary>
        /// Извлекает все необходимые сырые данные (замены и занятия) для указанной недели одним пакетом.
        /// </summary>
        private async Task<WeeklyScheduleRawData> FetchWeeklyScheduleDataAsync(Query request, Core.Entities.Semester semester, DateTime startOfWeekUtc, DateTime endOfWeekUtc, CancellationToken cancellationToken)
        {
            var overridesTask = _ctx.ScheduleOverrides.AsNoTracking()
                .Include(so => so.SubstituteDayOfWeek)
                .Where(so => so.OverrideDate >= startOfWeekUtc && so.OverrideDate < endOfWeekUtc && (so.GroupId == request.GroupId || so.GroupId == null))
                .ToListAsync(cancellationToken);

            var lessonsTask = _ctx.Schedules.AsNoTracking()
                .Where(s => s.GroupSubject.GroupId == request.GroupId && s.GroupSubject.SemesterId == semester.Id)
                .ProjectTo<ScheduleDataDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            await Task.WhenAll(overridesTask, lessonsTask);

            return new WeeklyScheduleRawData(overridesTask.Result, lessonsTask.Result);
        }

        // 4. МЕТОД ДЛЯ СБОРКИ РАСПИСАНИЯ
        /// <summary>
        /// Собирает список DTO дневных расписаний из сырых данных. Не выполняет запросов к БД.
        /// </summary>
        private List<DailyScheduleDto> BuildDailySchedules(WeeklyScheduleRawData rawData, Core.Entities.Group group, Core.Entities.Semester semester, DateTime startOfWeekInUkraine, List<Core.Entities.ApplicationDayOfWeek> daysOfWeek, TimeZoneInfo authoritativeTimeZone)
        {
            var overridesMap = rawData.Overrides.ToDictionary(
                so => DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(so.OverrideDate, authoritativeTimeZone)), 
                so => so
            );
            
            var semesterStartDate = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(semester.StartDate.ToUniversalTime(), authoritativeTimeZone));
            var dailySchedules = new List<DailyScheduleDto>();

            for (int i = 0; i < 7; i++)
            {
                var currentDateInUkraine = startOfWeekInUkraine.AddDays(i);
                var currentDateOnly = DateOnly.FromDateTime(currentDateInUkraine);
                
                var weekNumber = CalculateWeekNumber(semesterStartDate, currentDateOnly);
                var isEvenWeek = (weekNumber % 2) == 0;
                
                var dayOfWeekEntity = daysOfWeek[i]; // Используем предзагруженный список
                overridesMap.TryGetValue(currentDateOnly, out var scheduleOverride);
                
                var dayOfWeekToQueryId = scheduleOverride?.SubstituteDayOfWeekId ?? dayOfWeekEntity.Id;
                
                var lessonsForDay = (scheduleOverride is null || scheduleOverride.SubstituteDayOfWeekId.HasValue)
                    ? rawData.AllPossibleLessons
                        .Where(l => l.ApplicationDayOfWeekId == dayOfWeekToQueryId && l.IsEvenWeek == isEvenWeek)
                        .OrderBy(l => l.PairNumber)
                        .ToList()
                    : new List<ScheduleDataDto>();
                
                dailySchedules.Add(new DailyScheduleDto
                {
                    Date = currentDateOnly,
                    DayOfWeekName = dayOfWeekEntity.Name,
                    DayOfWeekAbbreviation = dayOfWeekEntity.Abbreviation,
                    GroupName = group.Name,
                    WeekNumber = weekNumber,
                    IsEvenWeek = isEvenWeek,
                    Lessons = _mapper.Map<List<LessonDto>>(lessonsForDay),
                    OverrideInfo = _mapper.Map<ScheduleOverrideInfoDto>(scheduleOverride)
                });
            }

            return dailySchedules;
        }

        // Вспомогательные методы остаются без изменений
        private int CalculateWeekNumber(DateOnly semesterStart, DateOnly targetDate)
        {
            var startDayOfWeek = (int)semesterStart.DayOfWeek == 0 ? 7 : (int)semesterStart.DayOfWeek;
            var daysToSubtract = startDayOfWeek - 1;
            var mondayOfFirstWeek = semesterStart.AddDays(-daysToSubtract);
            var totalDays = targetDate.DayNumber - mondayOfFirstWeek.DayNumber;
            return totalDays < 0 ? 1 : (totalDays / 7) + 1;
        }

        private WeeklyScheduleDto CreateEmptyWeeklySchedule(Core.Entities.Group group, DateTime mondayInUkraine)
        {
            return new WeeklyScheduleDto
            {
                GroupName = group.Name,
                WeekStartDate = DateOnly.FromDateTime(mondayInUkraine),
                WeekEndDate = DateOnly.FromDateTime(mondayInUkraine.AddDays(6)),
                DailySchedules = new List<DailyScheduleDto>()
            };
        }
    }
}