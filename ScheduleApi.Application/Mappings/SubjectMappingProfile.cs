using AutoMapper;
using ScheduleApi.Application.DTOs.Subject;

namespace ScheduleApi.Application.Mappings;

public class SubjectMappingProfile : Profile
{
    public SubjectMappingProfile()
    {
        CreateMap<CreateSubjectDto, Core.Entities.Subject>();
        
        CreateMap<Core.Entities.Subject, SubjectDto>()
            .ForMember(dest => dest.SubjectTypeName, 
                opt => opt.MapFrom(src => src.SubjectType.Name));
    }
}