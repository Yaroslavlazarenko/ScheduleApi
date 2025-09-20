using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.TeacherInfo;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.TeacherInfo.Commands;

public static class AddInfoToTeacher
{
    public record Command(int TeacherId, CreateTeacherInfoDto Model) : IRequest;

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
            var teacherExists = await _ctx.Teachers.AnyAsync(t => t.Id == request.TeacherId, cancellationToken);
            if (!teacherExists)
                throw new NotFoundException("Teacher not found");

            var infoTypeExists = await _ctx.InfoTypes.AnyAsync(it => it.Id == request.Model.InfoTypeId, cancellationToken);
            if (!infoTypeExists)
                throw new NotFoundException("Info type not found");

            var infoAlreadyExists = await _ctx.TeacherInfos
                .AnyAsync(ti => ti.TeacherId == request.TeacherId && ti.InfoTypeId == request.Model.InfoTypeId, cancellationToken);
            if (infoAlreadyExists)
                throw new BadRequestException($"Teacher info already exists");

            var teacherInfoEntity = _mapper.Map<Core.Entities.TeacherInfo>(request.Model);
            teacherInfoEntity.TeacherId = request.TeacherId;

            await _ctx.TeacherInfos.AddAsync(teacherInfoEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}