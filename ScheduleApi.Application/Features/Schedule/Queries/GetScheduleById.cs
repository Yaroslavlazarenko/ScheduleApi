using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Schedule.Queries;

public static class GetScheduleById
{
    public record Query(int Id) : IRequest<ScheduleDto>;

    private class Handler : IRequestHandler<Query, ScheduleDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<ScheduleDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var scheduleDto = await _ctx.Schedules
                .AsNoTracking()
                .Where(s => s.Id == request.Id)
                .ProjectTo<ScheduleDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (scheduleDto is null)
            {
                throw new NotFoundException("Schedule not found");
            }

            return scheduleDto;
        }
    }
}