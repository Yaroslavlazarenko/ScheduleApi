

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

public static class GetGroupDailySchedule
{
    public record Query(int GroupId, string TimeZoneId, DateTime? TargetDateTime) : IRequest<DailyScheduleDto>;

    // Вспомогательный record для передачи данных между методами
    private record SchedulePrerequisites(Core.Entities.Group Group, Core.Entities.ApplicationDayOfWeek? DayOfWeek, Core.Entities.Semester? Semester);

    private class Handler : IRequestHandler<Query, DailyScheduleDto>
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

        // 1. МЕТОД-ДИРИЖЕР
        public async Task<DailyScheduleDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var timeContext = _timeContextService.CreateContext(request.TargetDateTime, request.TimeZoneId);

            var prereqs = await GetSchedulePrerequisitesAsync(request.GroupId, timeContext, cancellationToken);
            if (prereqs.Semester is null || prereqs.DayOfWeek is null)
            {
                return CreateHeaderOnlySchedule(DateOnly.FromDateTime(timeContext.AuthoritativeTime), prereqs.DayOfWeek?.Name, prereqs.Group);
            }

            var (weekNumber, isEvenWeek) = CalculateWeekParity(prereqs.Semester, timeContext);

            var scheduleOverride = await GetScheduleOverrideAsync(request.GroupId, timeContext, cancellationToken);
            
            var dayOfWeekToQueryId = scheduleOverride?.SubstituteDayOfWeekId ?? prereqs.DayOfWeek.Id;

            var lessons = await GetLessonsAsync(prereqs.Group.Id, prereqs.Semester.Id, dayOfWeekToQueryId, isEvenWeek, scheduleOverride, cancellationToken);

            return new DailyScheduleDto
            {
                Date = DateOnly.FromDateTime(timeContext.AuthoritativeTime),
                DayOfWeekName = prereqs.DayOfWeek.Name,
                DayOfWeekAbbreviation = prereqs.DayOfWeek.Abbreviation,
                GroupName = prereqs.Group.Name,
                WeekNumber = weekNumber,
                IsEvenWeek = isEvenWeek,
                Lessons = lessons,
                OverrideInfo = _mapper.Map<ScheduleOverrideInfoDto>(scheduleOverride)
            };
        }

        // 2. МЕТОД ДЛЯ ПОЛУЧЕНИЯ ОСНОВНЫХ ДАННЫХ
        /// <summary>
        /// Асинхронно извлекает все необходимые для построения расписания сущности: группу, день недели и семестр.
        /// </summary>
        private async Task<SchedulePrerequisites> GetSchedulePrerequisitesAsync(int groupId, ScheduleTimeContext timeContext, CancellationToken cancellationToken)
        {
            var group = await _ctx.Groups.AsNoTracking()
                            .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken)
                        ?? throw new NotFoundException($"Group with ID {groupId} was not found.");
            
            var dayOfWeekId = (int)timeContext.AuthoritativeTime.DayOfWeek == 0 ? 7 : (int)timeContext.AuthoritativeTime.DayOfWeek;
            var dayOfWeek = await _ctx.ApplicationDaysOfWeek.FindAsync([dayOfWeekId], cancellationToken: cancellationToken);

            var semester = await _ctx.Semesters.AsNoTracking()
                .FirstOrDefaultAsync(s => s.StartDate <= timeContext.StartOfDayUtc && s.EndDate >= timeContext.StartOfDayUtc, cancellationToken);
        
            return new SchedulePrerequisites(group, dayOfWeek, semester);
        }
        
        // 3. МЕТОД ДЛЯ БИЗНЕС-ЛОГИКИ ВРЕМЕНИ
        /// <summary>
        /// Рассчитывает номер и четность недели на основе семестра и целевой даты.
        /// </summary>
        private (int WeekNumber, bool IsEvenWeek) CalculateWeekParity(Core.Entities.Semester semester, ScheduleTimeContext timeContext)
        {
            var authoritativeTimeZone = _timeContextService.GetAuthoritativeTimeZone();
            var semesterStartDateInUkraine = TimeZoneInfo.ConvertTimeFromUtc(semester.StartDate.ToUniversalTime(), authoritativeTimeZone);
            var semesterStartDate = DateOnly.FromDateTime(semesterStartDateInUkraine);
            var targetDate = DateOnly.FromDateTime(timeContext.AuthoritativeTime);

            var weekNumber = CalculateWeekNumber(semesterStartDate, targetDate);
            var isEvenWeek = (weekNumber % 2) == 0;
            
            return (weekNumber, isEvenWeek);
        }

        // 4. МЕТОД ДЛЯ ПОЛУЧЕНИЯ ЗАМЕН
        /// <summary>
        /// Находит замену в расписании (override) для указанной группы и дня.
        /// </summary>
        private async Task<Core.Entities.ScheduleOverride?> GetScheduleOverrideAsync(int groupId, ScheduleTimeContext timeContext, CancellationToken cancellationToken)
        {
            return await _ctx.ScheduleOverrides.AsNoTracking()
                .Include(so => so.SubstituteDayOfWeek)
                .FirstOrDefaultAsync(so => 
                    (so.OverrideDate >= timeContext.StartOfDayUtc && so.OverrideDate < timeContext.EndOfDayUtc) && 
                    (so.GroupId == groupId || so.GroupId == null), cancellationToken);
        }

        // 5. МЕТОД ДЛЯ ПОЛУЧЕНИЯ ЗАНЯТИЙ
        /// <summary>
        /// Извлекает список занятий (уроков) на основе всех рассчитанных параметров.
        /// </summary>
        private async Task<List<LessonDto>> GetLessonsAsync(int groupId, int semesterId, int dayOfWeekId, bool isEvenWeek, Core.Entities.ScheduleOverride? scheduleOverride, CancellationToken cancellationToken)
        {
            if (scheduleOverride != null && !scheduleOverride.SubstituteDayOfWeekId.HasValue)
            {
                // Если замена - это "выходной день", возвращаем пустой список.
                return [];
            }

            return await _ctx.Schedules.AsNoTracking()
                .Where(s => 
                    s.GroupSubject.GroupId == groupId && 
                    s.ApplicationDayOfWeekId == dayOfWeekId && 
                    s.GroupSubject.SemesterId == semesterId && 
                    s.IsEvenWeek == isEvenWeek)
                .OrderBy(s => s.Pair.Number)
                .ProjectTo<LessonDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
        
        // Вспомогательные методы
        private DailyScheduleDto CreateHeaderOnlySchedule(DateOnly targetDate, string? dayOfWeekName, Core.Entities.Group group)
        {
            dayOfWeekName ??= targetDate.DayOfWeek.ToString();
            return new DailyScheduleDto
            {
                Date = targetDate,
                DayOfWeekName = dayOfWeekName,
                DayOfWeekAbbreviation = dayOfWeekName.Length > 2 ? dayOfWeekName.Substring(0, 2) : dayOfWeekName,
                GroupName = group.Name,
                Lessons = new List<LessonDto>()
            };
        }
        
        private int CalculateWeekNumber(DateOnly semesterStart, DateOnly targetDate)
        {
            var startDayOfWeek = (int)semesterStart.DayOfWeek == 0 ? 7 : (int)semesterStart.DayOfWeek;
            var daysToSubtract = startDayOfWeek - 1;
            var mondayOfFirstWeek = semesterStart.AddDays(-daysToSubtract);
            var totalDays = targetDate.DayNumber - mondayOfFirstWeek.DayNumber;
            return totalDays < 0 ? 1 : (totalDays / 7) + 1;
        }
    }
}