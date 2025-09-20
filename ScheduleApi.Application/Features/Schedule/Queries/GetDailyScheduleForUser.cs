using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Application.DTOs.ScheduleOverride;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Schedule.Queries;

public static class GetDailyScheduleForUser
{
    public record Query(int UserId, DateTime? TargetDateTime) : IRequest<DailyScheduleDto>;

    private class Handler : IRequestHandler<Query, DailyScheduleDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<DailyScheduleDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _ctx.Users.AsNoTracking().Include(u => u.Region)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
                ?? throw new NotFoundException("User not found");
            
            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(user.Region.TimeZoneId);
            
            var userLocalTime = GetUserLocalTime(request.TargetDateTime, userTimeZone);
            var targetDate = DateOnly.FromDateTime(userLocalTime.DateTime);
            var targetDateTimeForQuery = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, 0, 0, 0, DateTimeKind.Utc);
            
            var actualDayOfWeekId = (int)targetDate.DayOfWeek == 0 ? 7 : (int)targetDate.DayOfWeek;
            var actualDayOfWeekEntity = await _ctx.ApplicationDaysOfWeek.FirstAsync(d => d.Id == actualDayOfWeekId, cancellationToken);

            var semester = await _ctx.Semesters.AsNoTracking()
                .FirstOrDefaultAsync(s => s.StartDate <= targetDateTimeForQuery && s.EndDate >= targetDateTimeForQuery, cancellationToken);

            if (semester is null)
                return CreateHeaderOnlySchedule(targetDate, actualDayOfWeekEntity);

            var semesterStartDate = DateOnly.FromDateTime(semester.StartDate.ToUniversalTime());
            var weekNumber = CalculateWeekNumber(semesterStartDate, targetDate);
            var isEvenWeek = (weekNumber % 2) == 0;
            
            var scheduleOverride = await _ctx.ScheduleOverrides.AsNoTracking()
                .Include(so => so.SubstituteDayOfWeek)
                .FirstOrDefaultAsync(so => so.OverrideDate == targetDateTimeForQuery && (so.GroupId == user.GroupId || so.GroupId == null), cancellationToken);

            var dayOfWeekToQuery = scheduleOverride?.SubstituteDayOfWeekId ?? actualDayOfWeekId;
            
            var lessonsWithUtcTime = new List<LessonDto>();

            if (scheduleOverride == null || scheduleOverride.SubstituteDayOfWeekId.HasValue)
            {
                lessonsWithUtcTime = await _ctx.Schedules.AsNoTracking()
                    .Where(s => s.GroupId == user.GroupId && s.ApplicationDayOfWeekId == dayOfWeekToQuery && s.SemesterId == semester.Id && s.IsEvenWeek == isEvenWeek)
                    .OrderBy(s => s.Pair.Number)
                    .ProjectTo<LessonDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
            
            var lessonsWithUserTime = lessonsWithUtcTime.Select(lesson =>
            {
                var startUtc = new DateTimeOffset(targetDate.ToDateTime(lesson.PairStartTime), TimeSpan.Zero);
                var endUtc = new DateTimeOffset(targetDate.ToDateTime(lesson.PairEndTime), TimeSpan.Zero);

                var startUser = TimeZoneInfo.ConvertTime(startUtc, userTimeZone);
                var endUser = TimeZoneInfo.ConvertTime(endUtc, userTimeZone);

                lesson.PairStartTime = TimeOnly.FromDateTime(startUser.DateTime);
                lesson.PairEndTime = TimeOnly.FromDateTime(endUser.DateTime);
                
                return lesson;
            }).ToList();
            
            return new DailyScheduleDto
            {
                Date = targetDate,
                DayOfWeekName = actualDayOfWeekEntity.Name,
                DayOfWeekAbbreviation = actualDayOfWeekEntity.Abbreviation,
                WeekNumber = weekNumber,
                IsEvenWeek = isEvenWeek,
                Lessons = lessonsWithUserTime,
                OverrideInfo = scheduleOverride != null ? new ScheduleOverrideInfoDto 
                { 
                    SubstitutedDayName = scheduleOverride.SubstituteDayOfWeek.Name,
                    Description = scheduleOverride.Description
                } : null
            };
        }
        
        private DailyScheduleDto CreateHeaderOnlySchedule(DateOnly targetDate, Core.Entities.ApplicationDayOfWeek dayOfWeekEntity)
        {
            return new DailyScheduleDto
            {
                Date = targetDate,
                DayOfWeekName = dayOfWeekEntity.Name,
                DayOfWeekAbbreviation = dayOfWeekEntity.Abbreviation,
                WeekNumber = 1,
                IsEvenWeek = false,
                Lessons = new List<LessonDto>()
            };
        }
        
        private DateTimeOffset GetUserLocalTime(DateTime? targetUtc, TimeZoneInfo userTimeZone)
        {
            var timeUtc = new DateTimeOffset(targetUtc ?? DateTime.UtcNow, TimeSpan.Zero);
            return TimeZoneInfo.ConvertTime(timeUtc, userTimeZone);
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