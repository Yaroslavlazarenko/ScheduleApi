using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Schedule.Queries;

public static class GetWeeklyScheduleForUser
{
    public record Query(int UserId, DateTime? TargetDate) : IRequest<WeeklyScheduleDto>;

    private class Handler : IRequestHandler<Query, WeeklyScheduleDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMediator _mediator;

        public Handler(ScheduleContext ctx, IMediator mediator)
        {
            _ctx = ctx;
            _mediator = mediator;
        }

        public async Task<WeeklyScheduleDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _ctx.Users.AsNoTracking().Include(u => u.Region)
                           .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
                       ?? throw new NotFoundException("User not found");
            
            if (user.Region is null)
            {
                throw new BadRequestException("User region is not set. Cannot determine time zone.");
            }

            var groupScheduleQuery = new GetGroupWeeklySchedule.Query(
                user.GroupId,
                user.Region.TimeZoneId,
                request.TargetDate
            );
            
            return await _mediator.Send(groupScheduleQuery, cancellationToken);
        }
    }
}