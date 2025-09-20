using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Semester;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Semester.Queries;

public static class GetSemestersList
{
    public record Query() : IRequest<List<SemesterDto>>;

    private class Handler : IRequestHandler<Query, List<SemesterDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<SemesterDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var list = await _ctx.Semesters
                .AsNoTracking()
                .OrderByDescending(s => s.StartDate)
                .ProjectTo<SemesterDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}