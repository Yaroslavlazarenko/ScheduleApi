using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.User;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.User.Queries;

public static class GetUserById
{
    public record Query(int Id) : IRequest<UserDto>;
    
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
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
            
            if (user is null)
            {
                throw new NotFoundException("Not found user");
            }
            
            var userDto = _mapper.Map<UserDto>(user);
            
            return userDto;
        }
    }
}