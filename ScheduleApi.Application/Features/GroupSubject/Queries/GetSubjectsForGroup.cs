using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.GroupSubject;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.GroupSubject.Queries;

public static class GetSubjectsForGroup
{
    public record Query(int GroupId) : IRequest<List<GroupSubjectDto>>;

    private class Handler : IRequestHandler<Query, List<GroupSubjectDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<GroupSubjectDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (!await _ctx.Groups.AnyAsync(g => g.Id == request.GroupId, cancellationToken))
                throw new NotFoundException("Group not found");

            var list = await _ctx.GroupSubjects
                .AsNoTracking()
                .Where(gs => gs.GroupId == request.GroupId)
                .Include(gs => gs.Semester)
                .Include(gs => gs.TeacherSubject).ThenInclude(ts => ts.Teacher)
                .Include(gs => gs.TeacherSubject).ThenInclude(ts => ts.Subject).ThenInclude(s => s.SubjectName)
                .ProjectTo<GroupSubjectDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}