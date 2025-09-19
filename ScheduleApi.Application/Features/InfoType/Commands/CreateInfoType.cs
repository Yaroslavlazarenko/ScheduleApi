using AutoMapper;
using MediatR;
using ScheduleApi.Application.DTOs.InfoType;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.InfoType.Commands;

public static class CreateInfoType
{
    public record Command(CreateInfoTypeDto Model) : IRequest<int>;

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
            var infoTypeEntity = _mapper.Map<Core.Entities.InfoType>(request.Model);

            await _ctx.InfoTypes.AddAsync(infoTypeEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return infoTypeEntity.Id;
        }
    }
}