using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.ScheduleOverride;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.ScheduleOverride.Queries;

public static class GetScheduleOverridesList
{
    // Фильтры по датам и группе
    public record Query(DateTime? StartDate, DateTime? EndDate, int? GroupId) : IRequest<List<ScheduleOverrideDto>>;

    private class Handler : IRequestHandler<Query, List<ScheduleOverrideDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<ScheduleOverrideDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _ctx.ScheduleOverrides.AsNoTracking();

            if (request.StartDate.HasValue)
                query = query.Where(so => so.OverrideDate >= request.StartDate.Value.Date);
            
            if (request.EndDate.HasValue)
                query = query.Where(so => so.OverrideDate <= request.EndDate.Value.Date);
            
            if (request.GroupId.HasValue)
                query = query.Where(so => so.GroupId == request.GroupId.Value || so.GroupId == null);
            
            return await query
                .OrderByDescending(so => so.OverrideDate)
                .ProjectTo<ScheduleOverrideDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}