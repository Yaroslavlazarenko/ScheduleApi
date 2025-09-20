

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Schedule.Queries;

public static class GetDailyScheduleForUser
{
    public record Query(int UserId, DateOnly TargetDate) : IRequest<List<ScheduleDto>>;

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
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user is null)
                throw new NotFoundException("User not found");

            var targetDateTime = request.TargetDate.ToDateTime(TimeOnly.MinValue);
            
            var semester = await _ctx.Semesters
                .AsNoTracking()
                .FirstOrDefaultAsync(s => 
                    s.StartDate <= targetDateTime 
                    && s.EndDate >= targetDateTime, cancellationToken);
            
            if (semester is null)
                return new List<ScheduleDto>();

            var semesterStartDate = DateOnly.FromDateTime(semester.StartDate);

            var overrideDate = request.TargetDate.ToDateTime(TimeOnly.MinValue);
            var scheduleOverride = await _ctx.ScheduleOverrides
                .AsNoTracking()
                .FirstOrDefaultAsync(so => so.OverrideDate == overrideDate && (so.GroupId == user.GroupId || so.GroupId == null), cancellationToken);

            if (scheduleOverride != null)
            {
                if (scheduleOverride.SubstituteDayOfWeekId.HasValue)
                {
                    var weekNumber = CalculateWeekNumber(semesterStartDate, request.TargetDate);
                    var isEvenWeek = (weekNumber % 2) == 0;
                    
                    return await GetScheduleEntriesAsync(user.GroupId, scheduleOverride.SubstituteDayOfWeekId.Value, semester.Id, isEvenWeek, cancellationToken);
                }
                
                return new List<ScheduleDto>();
            }

            var targetDayOfWeek = request.TargetDate.DayOfWeek;
            
            if (targetDayOfWeek == DayOfWeek.Saturday)
            {
                var weekNumber = CalculateWeekNumber(semesterStartDate, request.TargetDate);

                var weekBlock = (weekNumber - 1) / 5;

                var isEvenWeekForSubstitute = ((weekBlock + 1) % 2) == 0;
                
                var substituteDayId = ((weekNumber - 1) % 5) + 1;

                return await GetScheduleEntriesAsync(user.GroupId, substituteDayId, semester.Id, isEvenWeekForSubstitute, cancellationToken);
            }
            
            var dayOfWeekId = (int)targetDayOfWeek;
            if (dayOfWeekId == 0) dayOfWeekId = 7;

            var finalWeekNumber = CalculateWeekNumber(semesterStartDate, request.TargetDate);
            var finalIsEvenWeek = (finalWeekNumber % 2) == 0;
            
            return await GetScheduleEntriesAsync(user.GroupId, dayOfWeekId, semester.Id, finalIsEvenWeek, cancellationToken);
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