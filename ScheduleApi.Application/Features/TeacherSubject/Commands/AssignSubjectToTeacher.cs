using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.TeacherSubject;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.TeacherSubject.Commands;

public static class AssignSubjectToTeacher
{
    public record Command(int TeacherId, AssignSubjectToTeacherDto Model) : IRequest;

    private class Handler : IRequestHandler<Command>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await _ctx.Teachers.AnyAsync(t => t.Id == request.TeacherId, cancellationToken))
                throw new NotFoundException("Teacher not found");
            
            if (!await _ctx.Subjects.AnyAsync(s => s.Id == request.Model.SubjectId, cancellationToken))
                throw new NotFoundException("Subject not found");
            
            if (request.Model.SocialMediaTypesId.HasValue && 
                !await _ctx.SocialMediaTypes.AnyAsync(smt => smt.Id == request.Model.SocialMediaTypesId.Value, cancellationToken))
                throw new NotFoundException("Social media type not found");
            
            if (await _ctx.TeacherSubjects.AnyAsync(ts => ts.TeacherId == request.TeacherId && ts.SubjectId == request.Model.SubjectId, cancellationToken))
                throw new BadRequestException("This subject is already assigned to this teacher.");

            var newAssignment = _mapper.Map<Core.Entities.TeacherSubject>(request.Model);
            newAssignment.TeacherId = request.TeacherId;

            await _ctx.TeacherSubjects.AddAsync(newAssignment, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}