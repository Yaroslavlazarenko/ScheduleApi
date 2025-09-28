using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Schedule.Queries;

public static class GetSchedulesList
{
    // Запрос остается без изменений - API для клиента не меняется
    public record Query(int? GroupId, int? SemesterId, int? TeacherId) : IRequest<List<ScheduleDto>>;

    private class Handler : IRequestHandler<Query, List<ScheduleDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<ScheduleDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _ctx.Schedules.AsNoTracking();
            
            if (request.GroupId.HasValue)
                query = query.Where(s => s.GroupSubject.GroupId == request.GroupId.Value);
            
            if (request.SemesterId.HasValue)
                query = query.Where(s => s.GroupSubject.SemesterId == request.SemesterId.Value);

            if (request.TeacherId.HasValue)
                query = query.Where(s => s.GroupSubject.TeacherSubject.TeacherId == request.TeacherId.Value);
            
            query = query
                .Include(s => s.GroupSubject).ThenInclude(gs => gs.Group)
                .Include(s => s.GroupSubject).ThenInclude(gs => gs.Semester)
                .Include(s => s.GroupSubject).ThenInclude(gs => gs.TeacherSubject).ThenInclude(ts => ts.Teacher)
                .Include(s => s.GroupSubject).ThenInclude(gs => gs.TeacherSubject).ThenInclude(ts => ts.Subject).ThenInclude(sub => sub.SubjectName);

            return await query
                .OrderBy(s => s.ApplicationDayOfWeekId).ThenBy(s => s.Pair.Number)
                .ProjectTo<ScheduleDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}