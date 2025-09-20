using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Subject.Queries;

public static class GetSubjectsList
{
    public record Query() : IRequest<List<SubjectDto>>;

    private class Handler : IRequestHandler<Query, List<SubjectDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<SubjectDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var list = await _ctx.Subjects
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ProjectTo<SubjectDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}