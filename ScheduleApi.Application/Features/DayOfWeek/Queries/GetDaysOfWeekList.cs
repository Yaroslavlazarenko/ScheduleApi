using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.DayOfWeek;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.DayOfWeek.Queries;

public static class GetDaysOfWeekList
{
    public record Query() : IRequest<List<DayOfWeekDto>>;

    private class Handler : IRequestHandler<Query, List<DayOfWeekDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<DayOfWeekDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var list = await _ctx.ApplicationDaysOfWeek
                .AsNoTracking()
                .OrderBy(d => d.Id)
                .ProjectTo<DayOfWeekDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}