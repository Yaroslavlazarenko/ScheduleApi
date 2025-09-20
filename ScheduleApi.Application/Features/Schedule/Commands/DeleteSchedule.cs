using MediatR;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Schedule.Commands;

public static class DeleteSchedule
{
    public record Command(int Id) : IRequest;

    private class Handler : IRequestHandler<Command>
    {
        private readonly ScheduleContext _ctx;

        public Handler(ScheduleContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var scheduleEntity = await _ctx.Schedules.FindAsync(new object[] { request.Id }, cancellationToken);
            if (scheduleEntity is null)
                throw new NotFoundException("Schedule not found");

            _ctx.Schedules.Remove(scheduleEntity);
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}