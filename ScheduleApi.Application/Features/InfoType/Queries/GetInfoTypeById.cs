using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.InfoType;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.InfoType.Queries;

public static class GetInfoTypeById
{
    public record Query(int Id) : IRequest<InfoTypeDto>;

    private class Handler : IRequestHandler<Query, InfoTypeDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<InfoTypeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var infoType = await _ctx.InfoTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(it => it.Id == request.Id, cancellationToken);

            if (infoType is null)
            {
                throw new NotFoundException("InfoType not found");
            }

            return _mapper.Map<InfoTypeDto>(infoType);
        }
    }
}