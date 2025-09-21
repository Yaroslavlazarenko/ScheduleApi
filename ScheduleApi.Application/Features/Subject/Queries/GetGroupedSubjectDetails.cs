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
    public record Query(string Abbreviation) : IRequest<GroupedSubjectDetailsDto>;

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
                .Where(s => s.Abbreviation == request.Abbreviation)
                .Include(s => s.SubjectType)
                .Include(s => s.SubjectInfos).ThenInclude(si => si.InfoType)
                .Include(s => s.TeacherSubjects).ThenInclude(ts => ts.Teacher)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!subjects.Any())
            {
                throw new NotFoundException($"Subject with abbreviation '{request.Abbreviation}' not found");
            }

            var firstSubject = subjects.First();

            var resultDto = new GroupedSubjectDetailsDto
            {
                Name = firstSubject.Name,
                ShortName = firstSubject.ShortName,
                Abbreviation = firstSubject.Abbreviation,
                
                Variants = subjects.Select(s => new SubjectVariantDto
                {
                    Id = s.Id,
                    SubjectType = _mapper.Map<SubjectTypeDto>(s.SubjectType),
                    Teachers = s.TeacherSubjects
                                 .Select(ts => _mapper.Map<SubjectTeacherDto>(ts.Teacher))
                                 .ToList(),
                    Infos = _mapper.Map<List<SubjectInfoDto>>(s.SubjectInfos)
                }).ToList()
            };

            return resultDto;
        }
    }
}