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
            var groupedSubjects = await _ctx.Subjects
                .AsNoTracking()
                .Include(s => s.SubjectType)
                .OrderBy(s => s.Name)
                .GroupBy(s => new { s.Name, s.ShortName, s.Abbreviation })
                .Select(group => new GroupedSubjectDto
                {
                    Name = group.Key.Name,
                    ShortName = group.Key.ShortName,
                    Abbreviation = group.Key.Abbreviation,
                    Variants = group.Select(subjectInGroup => new SubjectVariantDto
                    {
                        Id = subjectInGroup.Id,
                        SubjectType = _mapper.Map<SubjectTypeDto>(subjectInGroup.SubjectType)
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return groupedSubjects;
        }
    }
}