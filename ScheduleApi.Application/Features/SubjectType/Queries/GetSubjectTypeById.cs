using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.SubjectType;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.SubjectType.Queries;

public static class GetSubjectTypeById
{
    public record Query(int Id) : IRequest<SubjectTypeDto>;

    private class Handler : IRequestHandler<Query, SubjectTypeDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<SubjectTypeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var subjectTypeDto = await _ctx.SubjectTypes
                .AsNoTracking()
                .Where(st => st.Id == request.Id)
                .ProjectTo<SubjectTypeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (subjectTypeDto is null)
            {
                throw new NotFoundException($"Subject type with ID {request.Id} not found.");
            }

            return subjectTypeDto;
        }
    }
}