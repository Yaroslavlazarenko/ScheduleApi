using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Application.DTOs.SubjectType;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Subject.Queries;

public static class GetSubjectsList
{
    public record Query() : IRequest<List<GroupedSubjectDto>>;

    private class Handler : IRequestHandler<Query, List<GroupedSubjectDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<GroupedSubjectDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var subjectNameGroups = await _ctx.SubjectNames
                .AsNoTracking()
                .OrderBy(sn => sn.FullName)
                .Select(sn => new GroupedSubjectDto
                {
                    SubjectNameId = sn.Id,
                    Name = sn.FullName,
                    ShortName = sn.ShortName,
                    Abbreviation = sn.Abbreviation
                })
                .ToListAsync(cancellationToken);

            return subjectNameGroups;
        }
    }
}