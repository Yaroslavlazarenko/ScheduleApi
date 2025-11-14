using AutoMapper;
using ScheduleApi.Application.DTOs.Subject;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class SubjectMappingProfile : Profile
{
    public SubjectMappingProfile()
    {
        int? groupId = null;

        CreateMap<CreateSubjectDto, Subject>();
        CreateMap<Subject, SubjectVariantDto>();
        CreateMap<Subject, SubjectNameDto>();
        
        CreateMap<Subject, SubjectDto>()
            .ForMember(dest => dest.Infos,
                opt => opt.MapFrom(src => src.SubjectInfos));
        
        CreateMap<Subject, SubjectDetailsDto>()
            .ForMember(dest => dest.Infos,
                opt => opt.MapFrom(src => src.SubjectInfos)) // Infos маппятся как и раньше
            
            .ForMember(dest => dest.Teachers,
                opt => opt.MapFrom(src =>
                    src.TeacherSubjects
                        .Where(ts => groupId == null || 
                                     ts.GroupSubjects.Any(gs => gs.GroupId == groupId))
                        .Select(ts => ts.Teacher)
                ));
    }
}