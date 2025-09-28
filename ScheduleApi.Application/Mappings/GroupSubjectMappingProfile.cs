using AutoMapper;
using ScheduleApi.Application.DTOs.GroupSubject;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class GroupSubjectMappingProfile : Profile
{
    public GroupSubjectMappingProfile()
    {
        CreateMap<GroupSubject, GroupSubjectDto>()
            .ForMember(dest => dest.SubjectName,
                opt => opt.MapFrom(src => src.Subject.SubjectName.FullName))
            .ForMember(dest => dest.TeacherFullName,
                opt => opt.MapFrom(src => $"{src.Teacher.LastName} {src.Teacher.FirstName} {src.Teacher.MiddleName}".Trim()));
    }
}