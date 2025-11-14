using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.Pair;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.Pair.Queries;

public static class GetPairById
{
    public record Query(int Id) : IRequest<PairDto>;

    private class Handler : IRequestHandler<Query, PairDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<PairDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var pairDto = await _ctx.Pairs
                .AsNoTracking()
                .Where(p => p.Id == request.Id)
                .ProjectTo<PairDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (pairDto is null)
            {
                throw new NotFoundException($"Pair with ID {request.Id} not found.");
            }

            return pairDto;
        }
    }
}