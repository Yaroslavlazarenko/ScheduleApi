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
    public record Query(string Abbreviation, int? GroupId) : IRequest<GroupedSubjectDetailsDto>;

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
            var subjects = await _ctx.Subjects
                .Where(s => s.SubjectName.Abbreviation == request.Abbreviation)
                .Include(s => s.SubjectName)
                .Include(s => s.SubjectType)
                .Include(s => s.SubjectInfos).ThenInclude(si => si.InfoType)
                .Include(s => s.TeacherSubjects
                    .Where(ts => !request.GroupId.HasValue ||
                                 ts.GroupSubjects.Any(gs => gs.GroupId == request.GroupId.Value)))
                .ThenInclude(ts => ts.Teacher)
                .ThenInclude(t => t.TeacherInfos)
                .ThenInclude(ti => ti.InfoType)

                .AsNoTracking()
                .ToListAsync(cancellationToken);
            
            if (!subjects.Any())
            {
                throw new NotFoundException($"Subject with abbreviation '{request.Abbreviation}' not found");
            }

            if (request.GroupId.HasValue && !subjects.Any(s => s.TeacherSubjects.Any()))
            {
                throw new NotFoundException($"Subject with abbreviation '{request.Abbreviation}' has no teachers for group with id {request.GroupId.Value}");
            }
            
            var firstSubject = subjects.First();

            var resultDto = new GroupedSubjectDetailsDto
            {
                Name = firstSubject.SubjectName.FullName,
                ShortName = firstSubject.SubjectName.ShortName,
                Abbreviation = firstSubject.SubjectName.Abbreviation,
                Variants = subjects.Select(s => new SubjectVariantDto
                {
                    Id = s.Id,
                    SubjectType = _mapper.Map<SubjectTypeDto>(s.SubjectType),
                    Teachers = s.TeacherSubjects
                        .Select(ts => _mapper.Map<TeacherDto>(ts.Teacher))
                        .ToList(),
                    Infos = _mapper.Map<List<SubjectInfoDto>>(s.SubjectInfos)
                }).ToList()
            };

            return resultDto;
        }
    }
}