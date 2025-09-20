

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
    public record Query(int UserId, DateTime? TargetDateTime) : IRequest<List<ScheduleDto>>;

    private class Handler : IRequestHandler<Query, List<ScheduleDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<ScheduleDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _ctx.Users
                .AsNoTracking()
                .Include(u => u.Region)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user is null)
                throw new NotFoundException("User not found");

            var userLocalTime = GetUserLocalTime(request.TargetDateTime, user.Region.Number);
            var targetDate = DateOnly.FromDateTime(userLocalTime.DateTime);
            
            var targetDateTimeForQuery = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, 0, 0, 0, DateTimeKind.Utc);

            var semester = await _ctx.Semesters
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.StartDate <= targetDateTimeForQuery && s.EndDate >= targetDateTimeForQuery, cancellationToken);
            
            if (semester is null)
                return new List<ScheduleDto>();

            var semesterStartDate = DateOnly.FromDateTime(semester.StartDate.ToUniversalTime());
            
            var scheduleOverride = await _ctx.ScheduleOverrides
                .AsNoTracking()
                .Include(so => so.SubstituteDayOfWeek)
                .FirstOrDefaultAsync(so => so.OverrideDate == targetDateTimeForQuery && (so.GroupId == user.GroupId || so.GroupId == null), cancellationToken);

            var weekNumber = CalculateWeekNumber(semesterStartDate, targetDate);
            var isEvenWeek = (weekNumber % 2) == 0;
            
            List<ScheduleDto> scheduleDtos;

            if (scheduleOverride != null)
            {
                if (scheduleOverride.SubstituteDayOfWeekId.HasValue)
                {
                    scheduleDtos = await GetScheduleEntriesAsync(user.GroupId, scheduleOverride.SubstituteDayOfWeekId.Value, semester.Id, isEvenWeek, cancellationToken);

                    var overrideInfo = new ScheduleOverrideInfoDto
                    {
                        SubstitutedDayName = scheduleOverride.SubstituteDayOfWeek!.Name,
                        Description = scheduleOverride.Description ?? "Перенос занять"
                    };
                    
                    foreach (var dto in scheduleDtos)
                    {
                        dto.OverrideInfo = overrideInfo;
                    }
                }
                else
                {
                    scheduleDtos = new List<ScheduleDto>();
                }
            }
            else
            {
                var dayOfWeekId = (int)targetDate.DayOfWeek == 0 ? 7 : (int)targetDate.DayOfWeek;
                scheduleDtos = await GetScheduleEntriesAsync(user.GroupId, dayOfWeekId, semester.Id, isEvenWeek, cancellationToken);
            }

            return scheduleDtos;
        }
        
        private async Task<List<ScheduleDto>> GetScheduleEntriesAsync(int groupId, int dayOfWeekId, int semesterId, bool isEvenWeek, CancellationToken cancellationToken)
        {
             return await _ctx.Schedules
                .AsNoTracking()
                .Where(s =>
                    s.GroupId == groupId &&
                    s.ApplicationDayOfWeekId == dayOfWeekId &&
                    s.SemesterId == semesterId &&
                    s.IsEvenWeek == isEvenWeek)
                .OrderBy(s => s.Pair.Number)
                .ProjectTo<ScheduleDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
        
        private DateTimeOffset GetUserLocalTime(DateTime? targetUtc, int offsetHours)
        {
            var timeUtc = targetUtc ?? DateTime.UtcNow;
            var userOffset = TimeSpan.FromHours(offsetHours);
            return new DateTimeOffset(timeUtc).ToOffset(userOffset);
        }
        
        private int CalculateWeekNumber(DateOnly semesterStart, DateOnly targetDate)
        {
             var startDayOfWeek = (int)semesterStart.DayOfWeek;
            var daysToSubtract = (startDayOfWeek == 0) ? 6 : startDayOfWeek - 1;
            var mondayOfFirstWeek = semesterStart.AddDays(-daysToSubtract);
            var totalDays = targetDate.DayNumber - mondayOfFirstWeek.DayNumber;
            if (totalDays < 0) return 1;
            return (totalDays / 7) + 1;
        }
    }
}