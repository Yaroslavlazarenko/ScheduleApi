using AutoMapper;
using ScheduleApi.Application.DTOs.TeacherInfo;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class TeacherInfoMappingProfile : Profile
{
    public TeacherInfoMappingProfile()
    {
        CreateMap<CreateTeacherInfoDto, TeacherInfo>();
        
        CreateMap<TeacherInfo, TeacherInfoDto>()
            .ForMember(dest => dest.InfoTypeName,
                opt => opt.MapFrom(src => src.InfoType.Name));
    }
}