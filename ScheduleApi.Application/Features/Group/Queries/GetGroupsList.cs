using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Group;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Group.Queries;

public static class GetGroupsList
{
    public record Query() : IRequest<List<GroupDto>>;

    private class Handler : IRequestHandler<Query, List<GroupDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<GroupDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var groupList = await _ctx.Groups
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .ProjectTo<GroupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return groupList;
        }
    }
}