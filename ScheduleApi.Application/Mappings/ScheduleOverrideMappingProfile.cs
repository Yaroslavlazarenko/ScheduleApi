using AutoMapper;
using ScheduleApi.Application.DTOs.ScheduleOverride;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class ScheduleOverrideMappingProfile : Profile
{
    public ScheduleOverrideMappingProfile()
    {
        CreateMap<MutateScheduleOverrideDto, ScheduleOverride>();

        CreateMap<ScheduleOverride, ScheduleOverrideDto>()
            .ForMember(dest => dest.OverrideTypeName,
                opt => opt.MapFrom(src => src.OverrideType.Name))
            .ForMember(dest => dest.SubstituteDayOfWeekName,
                opt => opt.MapFrom(src => src.SubstituteDayOfWeek != null ? src.SubstituteDayOfWeek.Name : null))
            .ForMember(dest => dest.GroupName,
                opt => opt.MapFrom(src => src.Group != null ? src.Group.Name : null));
    }
}