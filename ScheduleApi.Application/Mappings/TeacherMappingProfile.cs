using AutoMapper;
using ScheduleApi.Application.DTOs.Teacher;

namespace ScheduleApi.Application.Mappings;

public class TeacherMappingProfile : Profile
{
    public TeacherMappingProfile()
    {
        CreateMap<CreateTeacherDto, Core.Entities.Teacher>();

        CreateMap<Core.Entities.Teacher, TeacherDto>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName} {src.MiddleName}".Trim()))
            .ForMember(dest => dest.Infos,
                opt => opt.MapFrom(src => src.TeacherInfos));
    }
}