using MediatR;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.ScheduleOverride.Commands;

public static class DeleteScheduleOverride
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
            var entity = await _ctx.ScheduleOverrides.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
                throw new NotFoundException("Schedule not found");

            _ctx.ScheduleOverrides.Remove(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}