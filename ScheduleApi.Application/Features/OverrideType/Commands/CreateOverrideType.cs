using AutoMapper;
using MediatR;
using ScheduleApi.Application.DTOs.OverrideType;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.OverrideType.Commands;

public static class CreateOverrideType
{
    public record Command(CreateOverrideTypeDto Model) : IRequest<int>;

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
            var overrideTypeEntity = _mapper.Map<Core.Entities.OverrideType>(request.Model);

            await _ctx.OverrideTypes.AddAsync(overrideTypeEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return overrideTypeEntity.Id;
        }
    }
}