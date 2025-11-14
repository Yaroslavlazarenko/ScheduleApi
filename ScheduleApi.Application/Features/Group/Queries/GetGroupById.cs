using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Group;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Group.Queries;

public static class GetGroupById
{
    public record Query(int Id) : IRequest<GroupDto>;

    private class Handler : IRequestHandler<Query, GroupDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<GroupDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var groupDto = await _ctx.Groups
                .AsNoTracking()
                .Where(g => g.Id == request.Id)
                .ProjectTo<GroupDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (groupDto is null)
            {
                throw new NotFoundException($"Group with ID {request.Id} not found.");
            }
            
            return groupDto;
        }
    }
}