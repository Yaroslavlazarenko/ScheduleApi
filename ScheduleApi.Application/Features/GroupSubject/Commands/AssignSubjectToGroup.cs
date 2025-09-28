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
            
            if (!await _ctx.Semesters.AnyAsync(s => s.Id == request.Model.SemesterId, cancellationToken))
                throw new NotFoundException("Semester not found");
            
            var teacherSubject = await _ctx.TeacherSubjects
                .AsNoTracking()
                .FirstOrDefaultAsync(ts => ts.TeacherId == request.Model.TeacherId && ts.SubjectId == request.Model.SubjectId, cancellationToken);
            
            if (teacherSubject is null)
                throw new BadRequestException("This teacher is not assigned to this subject.");
            
            var assignmentExists = await _ctx.GroupSubjects
                .AnyAsync(gs => 
                    gs.GroupId == request.GroupId && 
                    gs.TeacherSubjectId == teacherSubject.Id && 
                    gs.SemesterId == request.Model.SemesterId, 
                    cancellationToken);

            if (assignmentExists)
                throw new BadRequestException("This assignment already exists for this group in this semester.");
            
            var newAssignment = new Core.Entities.GroupSubject
            {
                GroupId = request.GroupId,
                TeacherSubjectId = teacherSubject.Id,
                SemesterId = request.Model.SemesterId
            };

            await _ctx.GroupSubjects.AddAsync(newAssignment, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}