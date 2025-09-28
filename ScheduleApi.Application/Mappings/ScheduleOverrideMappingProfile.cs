using AutoMapper;
using ScheduleApi.Application.DTOs.ScheduleOverride;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class ScheduleOverrideMappingProfile : Profile
{
    public ScheduleOverrideMappingProfile()
    {
        CreateMap<MutateScheduleOverrideDto, ScheduleOverride>();

        CreateMap<ScheduleOverride, ScheduleOverrideInfoDto>()
            .ForMember(dest => dest.SubstitutedDayName, opt => opt.MapFrom(src => src.SubstituteDayOfWeek.Name));
        
        CreateMap<ScheduleOverride, ScheduleOverrideDto>()
            .ForMember(dest => dest.OverrideTypeName,
                opt => opt.MapFrom(src => src.OverrideType.Name))
            .ForMember(dest => dest.SubstituteDayOfWeekName,
                opt => opt.MapFrom(src => src.SubstituteDayOfWeek.Name))
            .ForMember(dest => dest.GroupName,
                opt => opt.MapFrom(src => src.Group.Name));
    }
}