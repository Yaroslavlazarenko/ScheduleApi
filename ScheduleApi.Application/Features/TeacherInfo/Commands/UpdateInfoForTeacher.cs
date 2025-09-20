using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.TeacherInfo;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.TeacherInfo.Commands;

public static class UpdateInfoForTeacher
{
    public record Command(int TeacherId, int InfoTypeId, UpdateTeacherInfoDto Model) : IRequest;

    private class Handler : IRequestHandler<Command>
    {
        private readonly ScheduleContext _ctx;

        public Handler(ScheduleContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var teacherInfo = await _ctx.TeacherInfos
                .FirstOrDefaultAsync(ti => ti.TeacherId == request.TeacherId && ti.InfoTypeId == request.InfoTypeId, cancellationToken);

            if (teacherInfo is null)
                throw new NotFoundException("Teacher not found");
            
            teacherInfo.Value = request.Model.Value;
            
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}