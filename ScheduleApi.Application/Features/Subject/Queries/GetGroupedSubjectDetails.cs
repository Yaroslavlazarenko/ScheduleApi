using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Application.DTOs.SubjectInfo;
using ScheduleApi.Application.DTOs.SubjectType;
using ScheduleApi.Application.DTOs.Teacher;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Subject.Queries;

public static class GetGroupedSubjectDetails
{
    public record Query(int SubjectNameId, int? GroupId) : IRequest<GroupedSubjectDetailsDto>;

    private class Handler : IRequestHandler<Query, GroupedSubjectDetailsDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<GroupedSubjectDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var subjectName = await _ctx.SubjectNames
                .AsNoTracking()
                .FirstOrDefaultAsync(sn => sn.Id == request.SubjectNameId, cancellationToken);

            if (subjectName == null)
            {
                throw new NotFoundException($"SubjectName with ID '{request.SubjectNameId}' not found");
            }
            
            var subjectVariants = await _ctx.Subjects
                .AsNoTracking()
                .Include(s => s.SubjectType)
                .Include(s => s.SubjectInfos).ThenInclude(si => si.InfoType)
                .Where(s => s.SubjectNameId == request.SubjectNameId)
                .ToListAsync(cancellationToken);

            if (!subjectVariants.Any())
            {
                throw new NotFoundException($"No Subject variants found for SubjectName with ID '{request.SubjectNameId}'");
            }
            
            var variantIds = subjectVariants.Select(v => v.Id).ToList();
            
            var teacherQuery = _ctx.TeacherSubjects
                .AsNoTracking()
                .Include(ts => ts.Teacher)
                .ThenInclude(t => t.TeacherInfos)
                .ThenInclude(ti => ti.InfoType)
                .Where(ts => variantIds.Contains(ts.SubjectId));
            
            if (request.GroupId.HasValue)
            {
                teacherQuery = teacherQuery.Where(ts => ts.GroupSubjects.Any(gs => gs.GroupId == request.GroupId.Value));
            }
            
            var teachersByVariant = (await teacherQuery.ToListAsync(cancellationToken))
                .GroupBy(ts => ts.SubjectId)
                .ToDictionary(g => g.Key, g => g.Select(ts => ts.Teacher).ToList());
            
            var resultDto = new GroupedSubjectDetailsDto
            {
                Name = subjectName.FullName,
                ShortName = subjectName.ShortName,
                Abbreviation = subjectName.Abbreviation,
                Variants = subjectVariants.Select(s => new SubjectVariantDto
                {
                    Id = s.Id,
                    SubjectType = _mapper.Map<SubjectTypeDto>(s.SubjectType),
                    Infos = _mapper.Map<List<SubjectInfoDto>>(s.SubjectInfos),
                    Teachers = teachersByVariant.TryGetValue(s.Id, out var teachers)
                        ? _mapper.Map<List<TeacherDto>>(teachers)
                        : new List<TeacherDto>()
                }).ToList()
            };
            
            if (request.GroupId.HasValue && !resultDto.Variants.Any(v => v.Teachers.Any()))
            {
                 throw new NotFoundException($"Subject '{subjectName.Abbreviation}' has no assigned teachers for the specified group.");
            }

            return resultDto;
        }
    }
}