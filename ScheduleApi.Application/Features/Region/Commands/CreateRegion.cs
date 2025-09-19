using AutoMapper;
using MediatR;
using ScheduleApi.Application.DTOs.Region;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Region.Commands;

public static class CreateRegion
{
    public record Command(CreateRegionDto Model) : IRequest<int>;

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
            var regionEntity = _mapper.Map<Core.Entities.Region>(request.Model);

            await _ctx.Regions.AddAsync(regionEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return regionEntity.Id;
        }
    }
}