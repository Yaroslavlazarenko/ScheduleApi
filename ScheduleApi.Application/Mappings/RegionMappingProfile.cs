using AutoMapper;
using ScheduleApi.Application.DTOs.Region;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class RegionMappingProfile : Profile
{
    public RegionMappingProfile()
    {
        CreateMap<CreateRegionDto, Region>();
        CreateMap<Region, RegionDto>();
    }
}