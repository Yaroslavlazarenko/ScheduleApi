using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.SocialMediaType;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.SocialMediaType.Queries;

public static class GetSocialMediaTypesList
{
    public record Query() : IRequest<List<SocialMediaTypeDto>>;

    private class Handler : IRequestHandler<Query, List<SocialMediaTypeDto>>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<List<SocialMediaTypeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var list = await _ctx.SocialMediaTypes
                .AsNoTracking()
                .OrderBy(smt => smt.Name)
                .ProjectTo<SocialMediaTypeDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}