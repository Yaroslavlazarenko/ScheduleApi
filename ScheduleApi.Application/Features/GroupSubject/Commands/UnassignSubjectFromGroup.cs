using MediatR;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.GroupSubject.Commands;

public static class UnassignSubjectFromGroup
{
    public record Command(int GroupId, int SubjectId, int TeacherId) : IRequest;

    private class Handler : IRequestHandler<Command>
    {
        private readonly ScheduleContext _ctx;

        public Handler(ScheduleContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var assignment = await _ctx.GroupSubjects
                .FindAsync(new object[] { request.GroupId, request.TeacherId, request.SubjectId }, cancellationToken);

            if (assignment is null)
                throw new NotFoundException($"{nameof(GroupSubject)} GroupId={request.GroupId}, SubjectId={request.SubjectId}, TeacherId={request.TeacherId}");

            _ctx.GroupSubjects.Remove(assignment);
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}