using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.TeacherSubject;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.TeacherSubject.Commands;

public static class UpdateSubjectForTeacher
{
    public record Command(int TeacherId, int SubjectId, UpdateTeacherSubjectDto Model) : IRequest;

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
            var assignment = await _ctx.TeacherSubjects
                .FirstOrDefaultAsync(ts => ts.TeacherId == request.TeacherId && ts.SubjectId == request.SubjectId, cancellationToken);

            if (assignment is null)
                throw new NotFoundException("Assignment not found");

            if (request.Model.SocialMediaTypesId.HasValue &&
                !await _ctx.SocialMediaTypes.AnyAsync(smt => smt.Id == request.Model.SocialMediaTypesId.Value, cancellationToken))
                throw new NotFoundException("Social media type not found");
            
            _mapper.Map(request.Model, assignment);
            
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}