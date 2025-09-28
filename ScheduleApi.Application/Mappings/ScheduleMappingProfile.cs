using AutoMapper;
using ScheduleApi.Application.DTOs.Schedule;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class ScheduleMappingProfile : Profile
{
    public ScheduleMappingProfile()
    {
        CreateMap<MutateScheduleDto, Schedule>();

        CreateMap<Schedule, ScheduleDto>()
            .ForMember(dest => dest.TeacherFullName,
                opt => opt.MapFrom(src =>
                    $"{src.GroupSubject.TeacherSubject.Teacher.LastName} {src.GroupSubject.TeacherSubject.Teacher.FirstName} {src.GroupSubject.TeacherSubject.Teacher.MiddleName}"
                        .Trim()))
            .ForMember(dest => dest.GroupName,
                opt => opt.MapFrom(src => src.GroupSubject.Group.Name))
            .ForMember(dest => dest.SubjectName,
                opt => opt.MapFrom(src => src.GroupSubject.TeacherSubject.Subject.SubjectName.FullName));
            
        CreateMap<Schedule, LessonDto>()
            .ForMember(dest => dest.SubjectName,
                opt => opt.MapFrom(src => src.GroupSubject.TeacherSubject.Subject.SubjectName.FullName))
            .ForMember(dest => dest.SubjectShortName,
                opt => opt.MapFrom(src => src.GroupSubject.TeacherSubject.Subject.SubjectName.ShortName))
            .ForMember(dest => dest.SubjectAbbreviation,
                opt => opt.MapFrom(src => src.GroupSubject.TeacherSubject.Subject.SubjectName.Abbreviation))
            .ForMember(dest => dest.SubjectTypeAbbreviation, 
                opt => opt.MapFrom(src => src.GroupSubject.TeacherSubject.Subject.SubjectType.Abbreviation))
            .ForMember(dest => dest.TeacherFullName, 
                opt => opt.MapFrom(src => $"{src.GroupSubject.TeacherSubject.Teacher.LastName} {src.GroupSubject.TeacherSubject.Teacher.FirstName} {src.GroupSubject.TeacherSubject.Teacher.MiddleName}"))
            .ForMember(dest => dest.LessonUrl, 
                opt => opt.MapFrom(src => src.GroupSubject.TeacherSubject.LessonUrl));
    }
}