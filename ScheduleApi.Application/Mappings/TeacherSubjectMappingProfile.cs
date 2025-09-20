using AutoMapper;
using ScheduleApi.Application.DTOs.TeacherSubject;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class TeacherSubjectMappingProfile : Profile
{
    public TeacherSubjectMappingProfile()
    {
        CreateMap<AssignSubjectToTeacherDto, TeacherSubject>();
        
        CreateMap<UpdateTeacherSubjectDto, TeacherSubject>();
        
        CreateMap<TeacherSubject, TeacherSubjectDto>()
            .ForMember(dest => dest.SubjectName,
                opt => opt.MapFrom(src => src.Subject.Name))
            .ForMember(dest => dest.SocialMediaTypeName,
                opt => opt.MapFrom(src => src.SocialMediaType.Name));
    }
}