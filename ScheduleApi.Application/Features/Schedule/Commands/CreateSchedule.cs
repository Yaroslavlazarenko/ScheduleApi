using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Schedule.Commands;

public static class CreateSchedule
{
    public record Command(MutateScheduleDto Model) : IRequest<int>;

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
            if (!await _ctx.Pairs.AnyAsync(p => p.Id == request.Model.PairId, cancellationToken))
                throw new NotFoundException("Pair not found");
            
            if (!await _ctx.ApplicationDaysOfWeek.AnyAsync(d => d.Id == request.Model.ApplicationDayOfWeekId, cancellationToken))
                throw new NotFoundException("Day of week not found");
            
            var groupSubject = await _ctx.GroupSubjects
                .AsNoTracking()
                .Include(gs => gs.Group)
                .FirstOrDefaultAsync(gs => gs.Id == request.Model.GroupSubjectId, cancellationToken);
            
            if (groupSubject is null)
                throw new NotFoundException("The specified assignment (GroupSubject) was not found.");

            var slotTaken = await _ctx.Schedules
                .Include(s => s.GroupSubject)
                .AnyAsync(s =>
                    s.GroupSubject.GroupId == groupSubject.GroupId &&
                    s.ApplicationDayOfWeekId == request.Model.ApplicationDayOfWeekId &&
                    s.PairId == request.Model.PairId &&
                    s.GroupSubject.SemesterId == groupSubject.SemesterId &&
                    s.IsEvenWeek == request.Model.IsEvenWeek, 
                    cancellationToken);

            if (slotTaken)
                throw new ValidationException("This time slot for this group is already taken in this semester.");

            var scheduleEntity = _mapper.Map<Core.Entities.Schedule>(request.Model);
            
            await _ctx.Schedules.AddAsync(scheduleEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return scheduleEntity.Id;
        }
    }
}