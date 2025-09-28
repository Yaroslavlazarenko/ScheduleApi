using AutoMapper;
using ScheduleApi.Application.DTOs.GroupSubject;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class GroupSubjectMappingProfile : Profile
{
    public GroupSubjectMappingProfile()
    {
        CreateMap<GroupSubject, GroupSubjectDto>()
            .ForMember(dest => dest.SubjectId,
                opt => opt.MapFrom(src => src.TeacherSubject.SubjectId))
            .ForMember(dest => dest.SubjectName,
                opt => opt.MapFrom(src => src.TeacherSubject.Subject.SubjectName.FullName))
            .ForMember(dest => dest.TeacherId,
                opt => opt.MapFrom(src => src.TeacherSubject.TeacherId))
            .ForMember(dest => dest.TeacherFullName,
                opt => opt.MapFrom(src =>
                    $"{src.TeacherSubject.Teacher.LastName} {src.TeacherSubject.Teacher.FirstName} {src.TeacherSubject.Teacher.MiddleName}"
                        .Trim()))
            .ForMember(dest => dest.SemesterId,
                opt => opt.MapFrom(src => src.SemesterId));
    }
}