using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Region;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Region.Queries;

public static class GetRegionsList
{
    public record Query() : IRequest<List<RegionDto>>;

    private class Handler : IRequestHandler<Query, List<RegionDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<RegionDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var list = await _ctx.Regions
                .AsNoTracking()
                .OrderBy(r => r.Number)
                .ProjectTo<RegionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}