using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Subject.Queries;

public static class GetSubjectById
{
    public record Query(int Id) : IRequest<SubjectDto>;

    private class Handler : IRequestHandler<Query, SubjectDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<SubjectDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var subject = await _ctx.Subjects
                .AsNoTracking()
                .Include(s => s.SubjectType) 
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

            if (subject is null)
            {
                throw new NotFoundException("Subject not found");
            }

            return _mapper.Map<SubjectDto>(subject);
        }
    }
}