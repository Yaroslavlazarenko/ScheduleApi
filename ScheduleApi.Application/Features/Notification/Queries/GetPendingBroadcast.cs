using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Notification;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Notification.Queries;

public static class GetPendingBroadcast
{
    public record Query : IRequest<PendingBroadcastDto?>;

    public class Handler : IRequestHandler<Query, PendingBroadcastDto?>
    {
        private readonly ScheduleContext _ctx;
        public Handler(ScheduleContext ctx) => _ctx = ctx;

        public async Task<PendingBroadcastDto?> Handle(Query request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var pendingBroadcast = await _ctx.Broadcasts
                .AsNoTracking()
                .Where(b => b.SentAt == null && b.ScheduledAt <= now)
                .OrderBy(b => b.ScheduledAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (pendingBroadcast == null)
            {
                return null;
            }

            var users = await _ctx.Users
                .AsNoTracking()
                .Where(u => u.TelegramId.HasValue)
                .Select(u => new UserBroadcastTargetDto { TelegramId = u.TelegramId.Value })
                .ToListAsync(cancellationToken);

            return new PendingBroadcastDto
            {
                Id = pendingBroadcast.Id,
                MessageText = pendingBroadcast.MessageText,
                Users = users
            };
        }
    }
}