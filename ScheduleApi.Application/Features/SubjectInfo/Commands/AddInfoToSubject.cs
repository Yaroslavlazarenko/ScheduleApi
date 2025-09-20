using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.SubjectInfo;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.SubjectInfo.Commands;

public static class AddInfoToSubject
{
    public record Command(int SubjectId, CreateSubjectInfoDto Model) : IRequest;

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
            var subjectExists = await _ctx.Subjects.AnyAsync(s => s.Id == request.SubjectId, cancellationToken);
            if (!subjectExists)
            {
                throw new NotFoundException("Subject not found");
            }
            
            var infoTypeExists = await _ctx.InfoTypes.AnyAsync(it => it.Id == request.Model.InfoTypeId, cancellationToken);
            if (!infoTypeExists)
            {
                throw new NotFoundException("Info type not found");
            }

            var infoAlreadyExists = await _ctx.SubjectInfos
                .AnyAsync(si => si.SubjectId == request.SubjectId && si.InfoTypeId == request.Model.InfoTypeId, cancellationToken);
            if (infoAlreadyExists)
            {
                throw new BadRequestException("Subject info already exists");
            }

            var subjectInfoEntity = _mapper.Map<Core.Entities.SubjectInfo>(request.Model);
            subjectInfoEntity.SubjectId = request.SubjectId;

            await _ctx.SubjectInfos.AddAsync(subjectInfoEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}