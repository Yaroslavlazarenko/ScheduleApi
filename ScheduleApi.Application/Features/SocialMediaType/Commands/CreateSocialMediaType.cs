using AutoMapper;
using MediatR;
using ScheduleApi.Application.DTOs.SocialMediaType;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.SocialMediaType.Commands;

public static class CreateSocialMediaType
{
    public record Command(CreateSocialMediaTypeDto Model) : IRequest<int>;

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
            var entity = _mapper.Map<Core.Entities.SocialMediaType>(request.Model);

            await _ctx.SocialMediaTypes.AddAsync(entity, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}