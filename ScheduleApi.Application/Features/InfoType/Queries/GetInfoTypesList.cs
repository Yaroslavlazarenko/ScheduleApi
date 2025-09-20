using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.InfoType;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.InfoType.Queries;

public static class GetInfoTypesList
{
    public record Query() : IRequest<List<InfoTypeDto>>;

    private class Handler : IRequestHandler<Query, List<InfoTypeDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<InfoTypeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var list = await _ctx.InfoTypes
                .AsNoTracking()
                .OrderBy(it => it.Name)
                .ProjectTo<InfoTypeDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}