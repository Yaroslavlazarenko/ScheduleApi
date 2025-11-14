using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var infoTypeDto = await _ctx.InfoTypes
                .AsNoTracking()
                .Where(it => it.Id == request.Id)
                .ProjectTo<InfoTypeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (infoTypeDto is null)
            {
                // Рекомендуется делать сообщения об ошибках более информативными
                throw new NotFoundException($"InfoType with ID {request.Id} not found.");
            }

            return infoTypeDto;
        }
    }
}