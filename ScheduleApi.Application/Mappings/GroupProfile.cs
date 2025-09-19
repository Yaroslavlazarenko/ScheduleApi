using AutoMapper;
using ScheduleApi.Application.DTOs;
using ScheduleApi.Application.DTOs.Group;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<CreateGroupDto, Group>();
        CreateMap<Group, GroupDto>();
    }
}