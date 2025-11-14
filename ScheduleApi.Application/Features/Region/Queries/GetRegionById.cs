using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var regionDto = await _ctx.Regions
                .AsNoTracking()
                .Where(r => r.Id == request.Id)
                .ProjectTo<RegionDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (regionDto is null)
            {
                throw new NotFoundException($"Region with ID {request.Id} not found.");
            }

            return regionDto;
        }
    }
}