using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.DayOfWeek;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.ApplicationDayOfWeek.Queries;

public static class GetDayOfWeekById
{
    public record Query(int Id) : IRequest<DayOfWeekDto>;

    private class Handler : IRequestHandler<Query, DayOfWeekDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<DayOfWeekDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var dayOfWeekDto = await _ctx.ApplicationDaysOfWeek
                .AsNoTracking()
                .Where(d => d.Id == request.Id)
                .ProjectTo<DayOfWeekDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (dayOfWeekDto is null)
            {
                throw new NotFoundException($"Day of week with ID {request.Id} not found.");
            }

            return dayOfWeekDto;
        }
    }
}