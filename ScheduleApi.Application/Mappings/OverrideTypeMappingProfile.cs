using AutoMapper;
using ScheduleApi.Application.DTOs.OverrideType;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class OverrideTypeMappingProfile : Profile
{
    public OverrideTypeMappingProfile()
    {
        CreateMap<CreateOverrideTypeDto, OverrideType>();
        CreateMap<OverrideType, OverrideTypeDto>();
    }
}