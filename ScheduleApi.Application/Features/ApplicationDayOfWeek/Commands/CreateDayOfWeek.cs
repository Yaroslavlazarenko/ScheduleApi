using AutoMapper;
using MediatR;
using ScheduleApi.Application.DTOs.DayOfWeek;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.ApplicationDayOfWeek.Commands;

public static class CreateDayOfWeek
{
    public record Command(CreateDayOfWeekDto Model) : IRequest<int>;

    private class Handler : IRequestHandler<Command, int>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var dayOfWeekEntity = _mapper.Map<Core.Entities.ApplicationDayOfWeek>(request.Model);

            await _ctx.ApplicationDaysOfWeek.AddAsync(dayOfWeekEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return dayOfWeekEntity.Id;
        }
    }
}