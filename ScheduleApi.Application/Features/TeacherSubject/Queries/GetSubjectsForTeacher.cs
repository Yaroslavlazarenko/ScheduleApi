using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.TeacherSubject;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.TeacherSubject.Queries;

public static class GetSubjectsForTeacher
{
    public record Query(int TeacherId) : IRequest<List<TeacherSubjectDto>>;

    private class Handler : IRequestHandler<Query, List<TeacherSubjectDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<TeacherSubjectDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (!await _ctx.Teachers.AnyAsync(t => t.Id == request.TeacherId, cancellationToken))
                throw new NotFoundException("Teacher not found");

            return await _ctx.TeacherSubjects
                .AsNoTracking()
                .Where(ts => ts.TeacherId == request.TeacherId)
                .ProjectTo<TeacherSubjectDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}