using AutoMapper;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class SubjectMappingProfile : Profile
{
    public SubjectMappingProfile()
    {
        CreateMap<CreateSubjectDto, Subject>();
        
        CreateMap<Subject, SubjectDto>()
            .ForMember(dest => dest.Infos,
                opt => opt.MapFrom(src => src.SubjectInfos));
    }
}