using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.ScheduleOverride;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.ScheduleOverride.Commands;

public static class UpdateScheduleOverride
{
    public record Command(int Id, MutateScheduleOverrideDto Model) : IRequest;

    private class Handler : IRequestHandler<Command>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.ScheduleOverrides.FindAsync(new object[] { request.Id }, cancellationToken);
            if (entity is null)
                throw new NotFoundException("Schedule override not found");

            if (!await _ctx.OverrideTypes.AnyAsync(ot => ot.Id == request.Model.OverrideTypeId, cancellationToken))
                throw new NotFoundException("Override type not found");
            
            if (request.Model.GroupId.HasValue && !await _ctx.Groups.AnyAsync(g => g.Id == request.Model.GroupId.Value, cancellationToken))
                throw new NotFoundException("Group not found");

            if (request.Model.SubstituteDayOfWeekId.HasValue && !await _ctx.ApplicationDaysOfWeek.AnyAsync(d => d.Id == request.Model.SubstituteDayOfWeekId.Value, cancellationToken))
                throw new NotFoundException("Day of week not found");
            
            _mapper.Map(request.Model, entity);
            entity.OverrideDate = entity.OverrideDate.Date;

            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}