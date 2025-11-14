using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.SocialMediaType;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.SocialMediaType.Queries;

public static class GetSocialMediaTypeById
{
    public record Query(int Id) : IRequest<SocialMediaTypeDto>;

    private class Handler : IRequestHandler<Query, SocialMediaTypeDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<SocialMediaTypeDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var socialMediaTypeDto = await _ctx.SocialMediaTypes
                .AsNoTracking()
                .Where(smt => smt.Id == request.Id)
                .ProjectTo<SocialMediaTypeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (socialMediaTypeDto is null)
            {
                throw new NotFoundException($"SocialMediaType with ID {request.Id} not found.");
            }

            return socialMediaTypeDto;
        }
    }
}