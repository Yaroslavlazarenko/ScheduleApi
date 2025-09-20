using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Pair;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Pair.Queries;

public static class GetPairsList
{
    public record Query() : IRequest<List<PairDto>>;

    private class Handler : IRequestHandler<Query, List<PairDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<PairDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _ctx.Pairs
                .AsNoTracking()
                .OrderBy(p => p.Number)
                .ProjectTo<PairDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}