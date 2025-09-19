using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.SubjectType;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.SubjectType.Queries;

public static class GetSubjectTypesList
{
    public record Query() : IRequest<List<SubjectTypeDto>>;

    private class Handler : IRequestHandler<Query, List<SubjectTypeDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<SubjectTypeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var list = await _ctx.SubjectTypes
                .AsNoTracking()
                .OrderBy(st => st.Name)
                .ProjectTo<SubjectTypeDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}