using AutoMapper;
using MediatR;
using ScheduleApi.Application.DTOs.Group;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Group.Commands;

public static class CreateGroup
{
    public record Command(CreateGroupDto Model) : IRequest<int>;

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
            var groupEntity = _mapper.Map<Core.Entities.Group>(request.Model);

            await _ctx.Groups.AddAsync(groupEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return groupEntity.Id;
        }
    }
}