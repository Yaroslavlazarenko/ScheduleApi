using AutoMapper;
using MediatR;
using ScheduleApi.Application.DTOs.Pair;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Pair.Commands;

public static class CreatePair
{
    public record Command(CreatePairDto Model) : IRequest<int>;

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
            var pairEntity = _mapper.Map<Core.Entities.Pair>(request.Model);
            await _ctx.Pairs.AddAsync(pairEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            return pairEntity.Id;
        }
    }
}