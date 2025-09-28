using MediatR;
using ScheduleApi.Application.DTOs.Notification;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Notification.Commands;

public static class CreateBroadcast
{
    public record Command(CreateBroadcastDto Dto) : IRequest<int>;

    public class Handler : IRequestHandler<Command, int>
    {
        private readonly ScheduleContext _ctx;

        public Handler(ScheduleContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            
            var scheduleTime = (request.Dto.ScheduledAt.HasValue && request.Dto.ScheduledAt.Value > now)
                ? request.Dto.ScheduledAt.Value.ToUniversalTime()
                : now;
            
            var newBroadcast = new Core.Entities.Broadcast
            {
                MessageText = request.Dto.MessageText,
                ScheduledAt = scheduleTime
            };

            _ctx.Broadcasts.Add(newBroadcast);
            
            await _ctx.SaveChangesAsync(cancellationToken);

            return newBroadcast.Id; 
        }
    }
}