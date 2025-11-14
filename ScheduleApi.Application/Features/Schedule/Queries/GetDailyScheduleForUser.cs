using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Schedule.Queries;

public static class GetDailyScheduleForUser
{
    public record Query(int UserId, DateTime? TargetDateTime) : IRequest<DailyScheduleDto>;

    private class Handler : IRequestHandler<Query, DailyScheduleDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMediator _mediator;

        public Handler(ScheduleContext ctx, IMediator mediator)
        {
            _ctx = ctx;
            _mediator = mediator;
        }

        public async Task<DailyScheduleDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _ctx.Users.AsNoTracking().Include(u => u.Region)
                           .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
                       ?? throw new NotFoundException("User not found");
            
            var groupScheduleQuery = new GetGroupDailySchedule.Query(
                user.GroupId,
                user.Region.TimeZoneId,
                request.TargetDateTime
            );
            
            return await _mediator.Send(groupScheduleQuery, cancellationToken);
        }
    }
}