using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.OverrideType;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.OverrideType.Queries;

public static class GetOverrideTypeById
{
    public record Query(int Id) : IRequest<OverrideTypeDto>;

    private class Handler : IRequestHandler<Query, OverrideTypeDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<OverrideTypeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var overrideTypeDto = await _ctx.OverrideTypes
                .AsNoTracking()
                .Where(ot => ot.Id == request.Id)
                .ProjectTo<OverrideTypeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (overrideTypeDto is null)
            {
                throw new NotFoundException($"Override type with ID {request.Id} not found.");
            }

            return overrideTypeDto;
        }
    }
}