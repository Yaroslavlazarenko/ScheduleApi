using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Application.DTOs.ScheduleOverride;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Schedule.Queries;

public static class GetGroupDailySchedule
{
    public record Query(int GroupId, string TimeZoneId, DateTime? TargetDateTime) : IRequest<DailyScheduleDto>;

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
            var group = await _ctx.Groups.AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

            if (group is null)
            {
                throw new NotFoundException($"Group with ID {request.GroupId} was not found.");
            }
            
            TimeZoneInfo groupTimeZone;
            try
            {
                groupTimeZone = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                throw new BadRequestException("Invalid TimeZoneId provided.");
            }
            
            var groupLocalTime = GetGroupLocalTime(request.TargetDateTime, groupTimeZone);
            var targetDate = DateOnly.FromDateTime(groupLocalTime.DateTime);
            var targetDateTimeForQuery = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, 0, 0, 0, DateTimeKind.Utc);
            
            var actualDayOfWeekId = (int)targetDate.DayOfWeek == 0 ? 7 : (int)targetDate.DayOfWeek;
            
            var actualDayOfWeekEntity = await _ctx.ApplicationDaysOfWeek
                .FirstOrDefaultAsync(d => d.Id == actualDayOfWeekId, cancellationToken);

            if (actualDayOfWeekEntity is null)
            {
                var dummyDayOfWeek = new Core.Entities.ApplicationDayOfWeek 
                { 
                    Name = targetDate.DayOfWeek.ToString(), 
                    Abbreviation = targetDate.DayOfWeek.ToString().Substring(0, 2)
                };
                return CreateHeaderOnlySchedule(targetDate, dummyDayOfWeek, group);
            }
            
            var semester = await _ctx.Semesters.AsNoTracking()
                .FirstOrDefaultAsync(s => s.StartDate <= targetDateTimeForQuery && s.EndDate >= targetDateTimeForQuery, cancellationToken);

            if (semester is null)
                return CreateHeaderOnlySchedule(targetDate, actualDayOfWeekEntity, group);
                
            var semesterStartDate = DateOnly.FromDateTime(semester.StartDate.ToUniversalTime());
            var weekNumber = CalculateWeekNumber(semesterStartDate, targetDate);
            var isEvenWeek = (weekNumber % 2) == 0;
            
            var scheduleOverride = await _ctx.ScheduleOverrides.AsNoTracking()
                .Include(so => so.SubstituteDayOfWeek)
                .FirstOrDefaultAsync(so => so.OverrideDate == targetDateTimeForQuery && (so.GroupId == request.GroupId || so.GroupId == null), cancellationToken);

            var dayOfWeekToQuery = scheduleOverride?.SubstituteDayOfWeekId ?? actualDayOfWeekId;
            
            var lessonsWithUtcTime = new List<LessonDto>();

            if (scheduleOverride == null || scheduleOverride.SubstituteDayOfWeekId.HasValue)
            {
                lessonsWithUtcTime = await _ctx.Schedules.AsNoTracking()
                    .Where(s => 
                        s.GroupSubject.GroupId == request.GroupId && 
                        s.ApplicationDayOfWeekId == dayOfWeekToQuery && 
                        s.GroupSubject.SemesterId == semester.Id && 
                        s.IsEvenWeek == isEvenWeek)
                    .Include(s => s.Pair)
                    .Include(s => s.GroupSubject)
                    .ThenInclude(gs => gs.TeacherSubject)
                    .ThenInclude(ts => ts.Teacher)
                    .Include(s => s.GroupSubject)
                    .ThenInclude(gs => gs.TeacherSubject)
                    .ThenInclude(ts => ts.Subject)
                    .ThenInclude(sub => sub.SubjectType)
                    .Include(s => s.GroupSubject)
                    .ThenInclude(gs => gs.TeacherSubject)
                    .ThenInclude(ts => ts.Subject)
                    .ThenInclude(sub => sub.SubjectName)
                    .OrderBy(s => s.Pair.Number)
                    .ProjectTo<LessonDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
            
            var lessonsWithGroupTime = lessonsWithUtcTime.Select(lesson =>
            {
                var startUtc = new DateTimeOffset(targetDate.ToDateTime(lesson.PairStartTime), TimeSpan.Zero);
                var endUtc = new DateTimeOffset(targetDate.ToDateTime(lesson.PairEndTime), TimeSpan.Zero);

                var startGroupLocal = TimeZoneInfo.ConvertTime(startUtc, groupTimeZone);
                var endGroupLocal = TimeZoneInfo.ConvertTime(endUtc, groupTimeZone);

                lesson.PairStartTime = TimeOnly.FromDateTime(startGroupLocal.DateTime);
                lesson.PairEndTime = TimeOnly.FromDateTime(endGroupLocal.DateTime);
                
                return lesson;
            }).ToList();
            
            return new DailyScheduleDto
            {
                Date = targetDate,
                DayOfWeekName = actualDayOfWeekEntity.Name,
                DayOfWeekAbbreviation = actualDayOfWeekEntity.Abbreviation,
                GroupName = group.Name,
                WeekNumber = weekNumber,
                IsEvenWeek = isEvenWeek,
                Lessons = lessonsWithGroupTime,
                OverrideInfo = scheduleOverride != null ? new ScheduleOverrideInfoDto 
                { 
                    SubstitutedDayName = scheduleOverride.SubstituteDayOfWeek.Name,
                    Description = scheduleOverride.Description
                } : null
            };
        }
        
        private DailyScheduleDto CreateHeaderOnlySchedule(DateOnly targetDate, Core.Entities.ApplicationDayOfWeek dayOfWeekEntity, Core.Entities.Group group)
        {
            return new DailyScheduleDto
            {
                Date = targetDate,
                DayOfWeekName = dayOfWeekEntity.Name,
                DayOfWeekAbbreviation = dayOfWeekEntity.Abbreviation,
                GroupName = group.Name,
                WeekNumber = 1,
                IsEvenWeek = false,
                Lessons = new List<LessonDto>()
            };
        }
        
        private DateTimeOffset GetGroupLocalTime(DateTime? targetUtc, TimeZoneInfo groupTimeZone)
        {
            var timeUtc = new DateTimeOffset(targetUtc ?? DateTime.UtcNow, TimeSpan.Zero);
            return TimeZoneInfo.ConvertTime(timeUtc, groupTimeZone);
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