using MediatR;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.GroupSubject.Commands;

public static class UnassignSubjectFromGroup
{
    public record Command(int GroupSubjectId) : IRequest;

    private class Handler : IRequestHandler<Command>
    {
        private readonly ScheduleContext _ctx;

        public Handler(ScheduleContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var assignment = await _ctx.GroupSubjects.FindAsync(request.GroupSubjectId, cancellationToken);

            if (assignment is null)
                throw new NotFoundException($"Assignment with Id={request.GroupSubjectId} not found.");

            _ctx.GroupSubjects.Remove(assignment);
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}