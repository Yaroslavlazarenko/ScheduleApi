using AutoMapper;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class SubjectMappingProfile : Profile
{
    public SubjectMappingProfile()
    {
        CreateMap<CreateSubjectDto, Subject>();
        CreateMap<Subject, SubjectVariantDto>();
        
        CreateMap<Subject, SubjectDto>()
            .ForMember(dest => dest.Infos,
                opt => opt.MapFrom(src => src.SubjectInfos));
        
        CreateMap<Subject, SubjectDetailsDto>()
            .ForMember(dest => dest.Infos,
                opt => opt.MapFrom(src => src.SubjectInfos))
            .ForMember(dest => dest.Teachers,
                opt => opt.MapFrom(src => src.TeacherSubjects.Select(ts => ts.Teacher)));
    }
}