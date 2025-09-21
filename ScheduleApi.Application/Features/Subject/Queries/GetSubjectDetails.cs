using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Subject.Queries;

public static class GetSubjectDetails
{
    public record Query(int Id) : IRequest<SubjectDetailsDto>;

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
                .Include(s => s.TeacherSubjects)
                .ThenInclude(ts => ts.Teacher)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (subject is null)
            {
                throw new NotFoundException($"Subject with id {request.Id} not found");
            }
            
            var resultDto = _mapper.Map<SubjectDetailsDto>(subject);

            return resultDto;
        }
    }
}