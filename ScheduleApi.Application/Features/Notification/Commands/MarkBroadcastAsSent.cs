using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Notification.Commands;

public static class MarkBroadcastAsSent
{
    public record Command(int BroadcastId) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly ScheduleContext _ctx;
        public Handler(ScheduleContext ctx) => _ctx = ctx;

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var broadcast = await _ctx.Broadcasts
                                .FirstOrDefaultAsync(b => b.Id == request.BroadcastId, cancellationToken)
                            ?? throw new NotFoundException($"Broadcast with ID {request.BroadcastId} not found.");

            if (broadcast.SentAt == null)
            {
                broadcast.SentAt = DateTime.UtcNow;
                await _ctx.SaveChangesAsync(cancellationToken);
            }
            
            return Unit.Value;
        }
    }
}