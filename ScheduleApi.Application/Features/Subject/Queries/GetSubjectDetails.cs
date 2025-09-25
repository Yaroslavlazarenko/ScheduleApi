using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Subject.Queries;

public static class GetSubjectDetails
{
    public record Query(int Id, int? GroupId) : IRequest<SubjectDetailsDto>;

    private class Handler : IRequestHandler<Query, SubjectDetailsDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<SubjectDetailsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var subject = await _ctx.Subjects
                .Include(s => s.SubjectType)
                .Include(s => s.SubjectInfos)
                .ThenInclude(si => si.InfoType)
                .Include(s => s.TeacherSubjects
                    .Where(ts => !request.GroupId.HasValue || 
                                 ts.GroupSubjects.Any(gs => gs.GroupId == request.GroupId.Value)))
                .ThenInclude(ts => ts.Teacher)
                .ThenInclude(t => t.TeacherInfos)
                .ThenInclude(ti => ti.InfoType)
        
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            
            if (subject is null)
            {
                throw new NotFoundException($"Subject with id {request.Id} not found");
            }

            if (request.GroupId.HasValue && !subject.TeacherSubjects.Any())
            {
                throw new NotFoundException($"Subject with id {request.Id} has no teachers assigned for group with id {request.GroupId.Value}");
            }
    
            var resultDto = _mapper.Map<SubjectDetailsDto>(subject);

            return resultDto;
        }
    }
}