using AutoMapper;
using ScheduleApi.Application.DTOs.InfoType;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class InfoTypeMappingProfile : Profile
{
    public InfoTypeMappingProfile()
    {
        CreateMap<CreateInfoTypeDto, InfoType>();
        CreateMap<InfoType, InfoTypeDto>();
    }
}