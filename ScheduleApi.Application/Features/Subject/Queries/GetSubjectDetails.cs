using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var resultDto = await _ctx.Subjects
                .AsNoTracking()
                .Where(s => s.Id == request.Id)
                .ProjectTo<SubjectDetailsDto>(_mapper.ConfigurationProvider,
                    new { groupId = request.GroupId }) 
                .FirstOrDefaultAsync(cancellationToken);

            if (resultDto is null)
            {
                throw new NotFoundException($"Subject with id {request.Id} not found");
            }
            
            if (request.GroupId.HasValue && !resultDto.Teachers.Any())
            {
                throw new NotFoundException($"Subject with id {request.Id} has no teachers assigned for group with id {request.GroupId.Value}");
            }
    
            return resultDto;
        }
    }
}