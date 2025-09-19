using AutoMapper;
using ScheduleApi.Application.DTOs.User;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserDto, User>();
        
        CreateMap<User, UserDto>();
    }
}