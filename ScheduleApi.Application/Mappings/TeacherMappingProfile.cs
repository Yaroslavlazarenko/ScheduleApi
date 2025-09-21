using AutoMapper;
using ScheduleApi.Application.DTOs.Teacher;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class TeacherMappingProfile : Profile
{
    public TeacherMappingProfile()
    {
        CreateMap<CreateTeacherDto, Teacher>();

        CreateMap<Teacher, TeacherDto>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName} {src.MiddleName}".Trim()))
            .ForMember(dest => dest.Infos,
                opt => opt.MapFrom(src => src.TeacherInfos));
        
        CreateMap<Teacher, SubjectTeacherDto>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.LastName} {src.FirstName} {src.MiddleName}".Trim()));
    }
}