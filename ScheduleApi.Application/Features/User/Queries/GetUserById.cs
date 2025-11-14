using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var userDto = await _ctx.Users
                .AsNoTracking()
                .Where(u => u.Id == request.Id)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (userDto is null)
            {
                throw new NotFoundException($"User with ID {request.Id} not found.");
            }
            
            return userDto;
        }
    }
}