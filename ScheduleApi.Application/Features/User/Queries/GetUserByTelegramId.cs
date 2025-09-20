using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.User;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.User.Queries;

public static class GetUserByTelegramId
{
    public record Query(long TelegramId) : IRequest<UserDto>;

    private class Handler : IRequestHandler<Query, UserDto>
    {
        private readonly ScheduleContext _ctx;
        private readonly IMapper _mapper;

        public Handler(ScheduleContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _ctx.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.TelegramId == request.TelegramId, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }
            
            return _mapper.Map<UserDto>(user);
        }
    }
}