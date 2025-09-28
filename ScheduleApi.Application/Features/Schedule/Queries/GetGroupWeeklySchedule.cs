using MediatR;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Core.Exceptions;

namespace ScheduleApi.Application.Features.Schedule.Queries;

public static class GetGroupWeeklySchedule
{
    public record Query(int GroupId, string TimeZoneId, DateTime? TargetDate) : IRequest<WeeklyScheduleDto>;

    private class Handler : IRequestHandler<Query, WeeklyScheduleDto>
    {
        private readonly IMediator _mediator;

        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<WeeklyScheduleDto> Handle(Query request, CancellationToken cancellationToken)
        {
            DateTime targetDateInUtc = request.TargetDate ?? DateTime.UtcNow;
            
            TimeZoneInfo groupTimeZone;
            try
            {
                groupTimeZone = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                throw new BadRequestException("Invalid TimeZoneId provided.");
            }
            
            var mondayOfTargetWeek = GetMondayOfTargetWeek(targetDateInUtc, groupTimeZone);

            var dailyScheduleTasks = new List<Task<DailyScheduleDto>>();
            
            for (int i = 0; i < 7; i++)
            {
                var currentDate = mondayOfTargetWeek.AddDays(i);
                var dailyQuery = new GetGroupDailySchedule.Query(request.GroupId, request.TimeZoneId, currentDate);
                dailyScheduleTasks.Add(_mediator.Send(dailyQuery, cancellationToken));
            }

            var dailySchedules = (await Task.WhenAll(dailyScheduleTasks)).ToList();

            var firstDaySchedule = dailySchedules.First();
            
            var weeklySchedule = new WeeklyScheduleDto
            {
                GroupName = firstDaySchedule.GroupName,
                WeekStartDate = DateOnly.FromDateTime(mondayOfTargetWeek),
                WeekEndDate = DateOnly.FromDateTime(mondayOfTargetWeek.AddDays(6)),
                WeekNumber = firstDaySchedule.WeekNumber,
                IsEvenWeek = firstDaySchedule.IsEvenWeek,
                TimeZoneId = request.TimeZoneId,
                DailySchedules = dailySchedules
            };

            return weeklySchedule;
        }

        private DateTime GetMondayOfTargetWeek(DateTime targetUtc, TimeZoneInfo timeZone)
        {
            var targetLocalTime = TimeZoneInfo.ConvertTimeFromUtc(targetUtc, timeZone);
            int diff = (7 + (int)targetLocalTime.DayOfWeek - (int)DayOfWeek.Monday) % 7;
            return targetLocalTime.AddDays(-1 * diff).Date;
        }
    }
}