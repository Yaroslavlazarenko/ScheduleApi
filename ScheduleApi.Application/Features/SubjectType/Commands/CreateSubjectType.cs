using AutoMapper;
using MediatR;
using ScheduleApi.Application.DTOs.SubjectType;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.SubjectType.Commands;

public static class CreateSubjectType
{
    public record Command(CreateSubjectTypeDto Model) : IRequest<int>;

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
            var subjectTypeEntity = _mapper.Map<Core.Entities.SubjectType>(request.Model);

            await _ctx.SubjectTypes.AddAsync(subjectTypeEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return subjectTypeEntity.Id;
        }
    }
}