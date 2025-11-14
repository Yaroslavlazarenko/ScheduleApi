using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Application.DTOs.SubjectType;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Subject.Queries;

public static class GetSubjectsList
{
    public record Query() : IRequest<List<SubjectNameDto>>;

    private class Handler : IRequestHandler<Query, List<SubjectNameDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<SubjectNameDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _ctx.SubjectNames
                .AsNoTracking()
                .OrderBy(sn => sn.FullName)
                .ProjectTo<SubjectNameDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}