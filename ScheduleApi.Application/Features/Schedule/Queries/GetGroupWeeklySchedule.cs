using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Application.DTOs.ScheduleOverride;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Schedule.Queries;

public static class GetGroupWeeklySchedule
{
    public record Query(int GroupId, string TimeZoneId, DateTime? TargetDate) : IRequest<WeeklyScheduleDto>;

    private class Handler : IRequestHandler<Query, WeeklyScheduleDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<WeeklyScheduleDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var group = await _ctx.Groups.AsNoTracking().FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);
            if (group is null) throw new NotFoundException($"Group with ID {request.GroupId} was not found.");

            var groupTimeZone = FindTimeZone(request.TimeZoneId);
            var mondayOfTargetWeek = GetMondayOfTargetWeek(request.TargetDate ?? DateTime.UtcNow, groupTimeZone);
            var sundayOfTargetWeek = mondayOfTargetWeek.AddDays(6);
            
            var mondayUtc = TimeZoneInfo.ConvertTimeToUtc(mondayOfTargetWeek);
            var sundayUtc = TimeZoneInfo.ConvertTimeToUtc(sundayOfTargetWeek.AddDays(1).AddTicks(-1));

            var semester = await _ctx.Semesters.AsNoTracking()
                .FirstOrDefaultAsync(s => s.StartDate <= mondayUtc && s.EndDate >= mondayUtc, cancellationToken);

            if (semester is null)
                return CreateEmptyWeeklySchedule(request, group, mondayOfTargetWeek);

            var overridesForWeek = await _ctx.ScheduleOverrides.AsNoTracking()
                .Include(so => so.SubstituteDayOfWeek)
                .Where(so => so.OverrideDate >= mondayUtc && so.OverrideDate <= sundayUtc && (so.GroupId == request.GroupId || so.GroupId == null))
                .ToDictionaryAsync(so => DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(so.OverrideDate, groupTimeZone)), cancellationToken);

            var allPossibleLessons = await _ctx.Schedules.AsNoTracking()
                .Where(s => s.GroupSubject.GroupId == request.GroupId && s.GroupSubject.SemesterId == semester.Id)
                .ProjectTo<ScheduleDataDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var dailySchedules = new List<DailyScheduleDto>();
            var weekNumber = 0;
            var isEvenWeek = false;

            for (int i = 0; i < 7; i++)
            {
                var currentDate = mondayOfTargetWeek.AddDays(i);
                var currentDateOnly = DateOnly.FromDateTime(currentDate);

                if (i == 0)
                {
                    var semesterStartDate = DateOnly.FromDateTime(semester.StartDate.ToUniversalTime());
                    weekNumber = CalculateWeekNumber(semesterStartDate, currentDateOnly);
                    isEvenWeek = (weekNumber % 2) == 0;
                }
                
                var dayOfWeekEntity = await _ctx.ApplicationDaysOfWeek.AsNoTracking().FirstAsync(d => d.Id == (i + 1), cancellationToken);
                overridesForWeek.TryGetValue(currentDateOnly, out var scheduleOverride);
                var dayOfWeekToQuery = scheduleOverride?.SubstituteDayOfWeekId ?? dayOfWeekEntity.Id;
                
                var lessonsForDayData = new List<ScheduleDataDto>();
                if (scheduleOverride is null || scheduleOverride.SubstituteDayOfWeekId.HasValue)
                {
                    lessonsForDayData = allPossibleLessons
                        .Where(l => l.ApplicationDayOfWeekId == dayOfWeekToQuery && l.IsEvenWeek == isEvenWeek)
                        .OrderBy(l => l.PairNumber)
                        .ToList();
                }
                
                var lessonsForDayDto = _mapper.Map<List<LessonDto>>(lessonsForDayData);
                
                foreach (var lesson in lessonsForDayDto)
                {
                    var startUtc = new DateTimeOffset(currentDateOnly.ToDateTime(lesson.PairStartTime), TimeSpan.Zero);
                    var endUtc = new DateTimeOffset(currentDateOnly.ToDateTime(lesson.PairEndTime), TimeSpan.Zero);
                    lesson.PairStartTime = TimeOnly.FromDateTime(TimeZoneInfo.ConvertTime(startUtc, groupTimeZone).DateTime);
                    lesson.PairEndTime = TimeOnly.FromDateTime(TimeZoneInfo.ConvertTime(endUtc, groupTimeZone).DateTime);
                }
                
                dailySchedules.Add(new DailyScheduleDto
                {
                    Date = currentDateOnly,
                    DayOfWeekName = dayOfWeekEntity.Name,
                    DayOfWeekAbbreviation = dayOfWeekEntity.Abbreviation,
                    GroupName = group.Name,
                    WeekNumber = weekNumber,
                    IsEvenWeek = isEvenWeek,
                    Lessons = lessonsForDayDto,
                    OverrideInfo = _mapper.Map<ScheduleOverrideInfoDto>(scheduleOverride)
                });
            }
            
            return new WeeklyScheduleDto
            {
                GroupName = group.Name,
                WeekStartDate = DateOnly.FromDateTime(mondayOfTargetWeek),
                WeekEndDate = DateOnly.FromDateTime(sundayOfTargetWeek),
                WeekNumber = weekNumber,
                IsEvenWeek = isEvenWeek,
                TimeZoneId = request.TimeZoneId,
                DailySchedules = dailySchedules
            };
        }

        private TimeZoneInfo FindTimeZone(string timeZoneId)
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId); }
            catch (TimeZoneNotFoundException) { throw new BadRequestException("Invalid TimeZoneId provided."); }
        }
        
        private DateTime GetMondayOfTargetWeek(DateTime targetUtc, TimeZoneInfo timeZone)
        {
            var targetLocalTime = TimeZoneInfo.ConvertTimeFromUtc(targetUtc, timeZone);
            int diff = (7 + (int)targetLocalTime.DayOfWeek - (int)DayOfWeek.Monday) % 7;
            return targetLocalTime.AddDays(-1 * diff).Date;
        }
        
        private int CalculateWeekNumber(DateOnly semesterStart, DateOnly targetDate)
        {
            var startDayOfWeek = (int)semesterStart.DayOfWeek == 0 ? 7 : (int)semesterStart.DayOfWeek;
            var daysToSubtract = startDayOfWeek - 1;
            var mondayOfFirstWeek = semesterStart.AddDays(-daysToSubtract);
            var totalDays = targetDate.DayNumber - mondayOfFirstWeek.DayNumber;
            return totalDays < 0 ? 1 : (totalDays / 7) + 1;
        }

        private WeeklyScheduleDto CreateEmptyWeeklySchedule(Query request, Core.Entities.Group group, DateTime monday)
        {
            return new WeeklyScheduleDto
            {
                GroupName = group.Name,
                WeekStartDate = DateOnly.FromDateTime(monday),
                WeekEndDate = DateOnly.FromDateTime(monday.AddDays(6)),
                WeekNumber = 1, IsEvenWeek = false, TimeZoneId = request.TimeZoneId,
                DailySchedules = new List<DailyScheduleDto>()
            };
        }
    }
}