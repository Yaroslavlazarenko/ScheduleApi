using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.ScheduleOverride;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.ScheduleOverride.Queries;

public static class GetScheduleOverrideById
{
    public record Query(int Id) : IRequest<ScheduleOverrideDto>;

    private class Handler : IRequestHandler<Query, ScheduleOverrideDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<ScheduleOverrideDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var overrideDto = await _ctx.ScheduleOverrides
                .AsNoTracking()
                .Where(so => so.Id == request.Id)
                .ProjectTo<ScheduleOverrideDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (overrideDto is null)
            {
                throw new NotFoundException("Override not found");
            }

            return overrideDto;
        }
    }
}