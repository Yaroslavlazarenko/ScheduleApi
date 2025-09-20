using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.GroupSubject;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.GroupSubject.Commands;

public static class AssignSubjectToGroup
{
    public record Command(int GroupId, AssignSubjectToGroupDto Model) : IRequest;

    private class Handler : IRequestHandler<Command>
    {
        private readonly ScheduleContext _ctx;

        public Handler(ScheduleContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await _ctx.Groups.AnyAsync(g => g.Id == request.GroupId, cancellationToken))
                throw new NotFoundException("Group not found");
            
            if (!await _ctx.Subjects.AnyAsync(s => s.Id == request.Model.SubjectId, cancellationToken))
                throw new NotFoundException("Subject not found");

            if (!await _ctx.Teachers.AnyAsync(t => t.Id == request.Model.TeacherId, cancellationToken))
                throw new NotFoundException("Teacher not found");
            
            var teacherCanTeachSubject = await _ctx.TeacherSubjects
                .AnyAsync(ts => ts.TeacherId == request.Model.TeacherId && ts.SubjectId == request.Model.SubjectId, cancellationToken);
            
            if (!teacherCanTeachSubject)
                throw new BadRequestException("This teacher is not assigned to this subject.");
            
            var assignmentExists = await _ctx.GroupSubjects
                .AnyAsync(gs => gs.GroupId == request.GroupId && gs.SubjectId == request.Model.SubjectId && gs.TeacherId == request.Model.TeacherId, cancellationToken);
            if (assignmentExists)
                throw new BadRequestException("This assignment already exists for this group.");
            
            var newAssignment = new Core.Entities.GroupSubject
            {
                GroupId = request.GroupId,
                SubjectId = request.Model.SubjectId,
                TeacherId = request.Model.TeacherId
            };

            await _ctx.GroupSubjects.AddAsync(newAssignment, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}