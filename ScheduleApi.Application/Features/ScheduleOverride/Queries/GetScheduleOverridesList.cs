using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Application.DTOs.ScheduleOverride;
using ScheduleApi.Application.Services;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.ScheduleOverride.Queries;

public static class GetScheduleOverridesList
{
    public record Query(ScheduleOverrideFilterDto Filter) : IRequest<List<ScheduleOverrideDto>>;

    private class Handler : IRequestHandler<Query, List<ScheduleOverrideDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;
        private readonly IScheduleTimeContextService _timeContextService;

        public Handler(ScheduleContext ctx, IMapper mapper, IScheduleTimeContextService timeContextService)
        {
            _ctx = ctx;
            _mapper = mapper;
            _timeContextService = timeContextService;
        }

        public async Task<List<ScheduleOverrideDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var filter = request.Filter;
            var query = _ctx.ScheduleOverrides.AsNoTracking();
            
            var utcRange = _timeContextService.CreateUtcDateRange(filter.StartDate, filter.EndDate, filter.TimeZoneId);

            if (utcRange.StartDateUtc.HasValue)
                query = query.Where(so => so.OverrideDate >= utcRange.StartDateUtc.Value);
            
            if (utcRange.EndDateUtc.HasValue)
                query = query.Where(so => so.OverrideDate < utcRange.EndDateUtc.Value);
            
            if (filter.GroupId.HasValue)
                query = query.Where(so => so.GroupId == filter.GroupId.Value || so.GroupId == null);
            
            return await query
                .OrderByDescending(so => so.OverrideDate)
                .ProjectTo<ScheduleOverrideDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}