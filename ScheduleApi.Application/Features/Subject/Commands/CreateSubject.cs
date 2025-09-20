using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Subject.Commands;

public static class CreateSubject
{
    public record Command(CreateSubjectDto Model) : IRequest<int>;

    private class Handler : IRequestHandler<Command, int>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var subjectTypeExists = await _ctx.SubjectTypes
                .AnyAsync(st => st.Id == request.Model.SubjectTypeId, cancellationToken);

            if (!subjectTypeExists)
            {
                throw new NotFoundException("Subject type not found");
            }

            var subjectEntity = _mapper.Map<Core.Entities.Subject>(request.Model);

            await _ctx.Subjects.AddAsync(subjectEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return subjectEntity.Id;
        }
    }
}