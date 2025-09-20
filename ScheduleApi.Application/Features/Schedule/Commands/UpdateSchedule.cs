using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Schedule.Commands;

public static class UpdateSchedule
{
    public record Command(int Id, MutateScheduleDto Model) : IRequest;

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
            var scheduleEntity = await _ctx.Schedules.FindAsync(new object[] { request.Id }, cancellationToken);
            
            if (scheduleEntity is null)
                throw new NotFoundException("Schedule not found");
            
            if (!await _ctx.Groups.AnyAsync(g => g.Id == request.Model.GroupId, cancellationToken))
                throw new NotFoundException("Group not found");
            
            if (!await _ctx.Teachers.AnyAsync(t => t.Id == request.Model.TeacherId, cancellationToken))
                throw new NotFoundException("Teacher not found");
            
            if (!await _ctx.Pairs.AnyAsync(p => p.Id == request.Model.PairId, cancellationToken))
                throw new NotFoundException("Pair not found");

            if (!await _ctx.Subjects.AnyAsync(s => s.Id == request.Model.SubjectId, cancellationToken))
                throw new NotFoundException("Subject not found");

            if (!await _ctx.Semesters.AnyAsync(s => s.Id == request.Model.SemesterId, cancellationToken))
                throw new NotFoundException("Semester not found");

            var assignmentExists = await _ctx.GroupSubjects.AnyAsync(gs =>
                gs.GroupId == request.Model.GroupId &&
                gs.TeacherId == request.Model.TeacherId &&
                gs.SubjectId == request.Model.SubjectId, cancellationToken);
            if (!assignmentExists)
                throw new ValidationException("The teacher is not assigned to this subject for this group.");

            var slotTaken = await _ctx.Schedules.AnyAsync(s =>
                s.Id != request.Id &&
                s.GroupId == request.Model.GroupId &&
                s.ApplicationDayOfWeekId == request.Model.ApplicationDayOfWeekId &&
                s.PairId == request.Model.PairId &&
                s.SemesterId == request.Model.SemesterId &&
                s.IsEvenWeek == request.Model.IsEvenWeek, cancellationToken);
            if (slotTaken)
                throw new ValidationException("Это время в расписании для данной группы уже занято другой записью.");
            
            _mapper.Map(request.Model, scheduleEntity);

            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}