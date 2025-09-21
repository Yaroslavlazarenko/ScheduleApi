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
                opt => opt.MapFrom(src => $"{src.Teacher.LastName} {src.Teacher.FirstName} {src.Teacher.MiddleName}".Trim()));
        
        CreateMap<Schedule, LessonDto>()
            .ForMember(dest => dest.SubjectTypeAbbreviation, 
                opt => opt.MapFrom(src => src.Subject.SubjectType.Abbreviation))
            .ForMember(dest => dest.TeacherFullName, 
                opt => opt.MapFrom(src => $"{src.Teacher.LastName} {src.Teacher.FirstName.Substring(0, 1)}. {src.Teacher.MiddleName.Substring(0, 1)}."))
            .ForMember(dest => dest.LessonUrl, 
                opt => opt.MapFrom(src => 
                    src.Teacher.TeacherSubjects.FirstOrDefault(ts => ts.SubjectId == src.SubjectId)!.LessonUrl));
    }
}