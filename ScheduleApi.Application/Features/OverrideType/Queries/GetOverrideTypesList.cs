using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.OverrideType;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.OverrideType.Queries;

public static class GetOverrideTypesList
{
    public record Query() : IRequest<List<OverrideTypeDto>>;

    private class Handler : IRequestHandler<Query, List<OverrideTypeDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<OverrideTypeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var list = await _ctx.OverrideTypes
                .AsNoTracking()
                .OrderBy(ot => ot.Name)
                .ProjectTo<OverrideTypeDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}