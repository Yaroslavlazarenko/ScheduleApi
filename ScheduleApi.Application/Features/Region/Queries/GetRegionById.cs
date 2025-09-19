using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Region;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Region.Queries;

public static class GetRegionById
{
    public record Query(int Id) : IRequest<RegionDto>;

    private class Handler : IRequestHandler<Query, RegionDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<RegionDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var region = await _ctx.Regions
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (region is null)
            {
                throw new NotFoundException("Region not found");
            }

            return _mapper.Map<RegionDto>(region);
        }
    }
}