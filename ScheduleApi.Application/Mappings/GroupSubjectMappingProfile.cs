using AutoMapper;
using ScheduleApi.Application.DTOs.GroupSubject;

namespace ScheduleApi.Application.Mappings;

public class GroupSubjectMappingProfile : Profile
{
    public GroupSubjectMappingProfile()
    {
        CreateMap<Core.Entities.GroupSubject, GroupSubjectDto>()
            .ForMember(dest => dest.SubjectName,
                opt => opt.MapFrom(src => src.Subject.Name))
            .ForMember(dest => dest.TeacherFullName,
                opt => opt.MapFrom(src => $"{src.Teacher.LastName} {src.Teacher.FirstName} {src.Teacher.MiddleName}".Trim()));
    }
}