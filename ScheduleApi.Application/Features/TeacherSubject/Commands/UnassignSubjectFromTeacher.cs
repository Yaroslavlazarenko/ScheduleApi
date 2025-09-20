using MediatR;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.TeacherSubject.Commands;

public static class UnassignSubjectFromTeacher
{
    public record Command(int TeacherId, int SubjectId) : IRequest;

    private class Handler : IRequestHandler<Command>
    {
        private readonly ScheduleContext _ctx;

        public Handler(ScheduleContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var assignment = await _ctx.TeacherSubjects
                .FindAsync(new object[] { request.TeacherId, request.SubjectId }, cancellationToken);

            if (assignment is null)
                throw new NotFoundException("Assignment not found");

            _ctx.TeacherSubjects.Remove(assignment);
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}