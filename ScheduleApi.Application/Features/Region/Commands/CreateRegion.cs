using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Region;
using ScheduleApi.Core.Exceptions;
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
            var alreadyExists = await _ctx.Regions
                .AnyAsync(r => r.TimeZoneId == request.Model.TimeZoneId, cancellationToken);

            if (alreadyExists)
            {
                throw new BadRequestException($"Регіон з TimeZoneId '{request.Model.TimeZoneId}' вже існує.");
            }

            var regionEntity = _mapper.Map<Core.Entities.Region>(request.Model);

            await _ctx.Regions.AddAsync(regionEntity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return regionEntity.Id;
        }
    }
}