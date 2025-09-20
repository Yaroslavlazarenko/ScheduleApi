using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.User;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.User.Commands;

public static class ChangeUserGroup
{
    public record Command(int UserId, ChangeUserGroupDto Model) : IRequest;

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
            
            var groupExists = await _ctx.Groups.AnyAsync(g => g.Id == request.Model.NewGroupId, cancellationToken);
            if (!groupExists)
                throw new NotFoundException("Group not found");
            
            user.GroupId = request.Model.NewGroupId;
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}