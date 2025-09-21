using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ScheduleApi.Application.DTOs.User;
using ScheduleApi.Core.Exceptions;
using ScheduleBotApi.Infrastructure.Contexts;

namespace ScheduleApi.Application.Features.User.Commands;

public static class CreateUser
{
    public record Command(CreateUserDto Model) : IRequest<int>; 
    
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
            var userExists = await _ctx.Users
                .AnyAsync(u => u.TelegramId == request.Model.TelegramId, cancellationToken);
            
            if (userExists)
            {
                throw new BadRequestException($"User with TelegramId '{request.Model.TelegramId}' already exists.");
            }

            var entity = _mapper.Map<Core.Entities.User>(request.Model);
            
            _ctx.Users.Add(entity);

            await _ctx.SaveChangesAsync(cancellationToken);
            
            return entity.Id; 
        }
    }
}