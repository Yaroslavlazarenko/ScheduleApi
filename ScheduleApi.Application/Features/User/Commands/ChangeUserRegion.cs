using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.User;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.User.Commands;

public static class ChangeUserRegion
{
    public record Command(int UserId, ChangeUserRegionDto Model) : IRequest;

    private class Handler : IRequestHandler<Command>
    {
        private readonly ScheduleContext _ctx;

        public Handler(ScheduleContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _ctx.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
            if (user is null)
                throw new NotFoundException("User not found");

            var regionExists = await _ctx.Regions.AnyAsync(r => r.Id == request.Model.NewRegionId, cancellationToken);
            if (!regionExists)
                throw new NotFoundException("Region not found");
            
            user.RegionId = request.Model.NewRegionId;
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}