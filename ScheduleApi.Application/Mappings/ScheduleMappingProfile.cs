using AutoMapper;
using ScheduleApi.Application.DTOs.Schedule;

namespace ScheduleApi.Application.Mappings;

public class ScheduleMappingProfile : Profile
{
    public ScheduleMappingProfile()
    {
        CreateMap<MutateScheduleDto, Core.Entities.Schedule>();

        CreateMap<Core.Entities.Schedule, ScheduleDto>()
            .ForMember(dest => dest.DayOfWeekName, opt => opt.MapFrom(src => src.ApplicationDayOfWeek.Name))
            .ForMember(dest => dest.PairNumber, opt => opt.MapFrom(src => src.Pair.Number))
            .ForMember(dest => dest.PairStartTime, opt => opt.MapFrom(src => src.Pair.StartTime))
            .ForMember(dest => dest.PairEndTime, opt => opt.MapFrom(src => src.Pair.EndTime))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name))
            .ForMember(dest => dest.TeacherFullName, opt => opt.MapFrom(src => $"{src.Teacher.LastName} {src.Teacher.FirstName} {src.Teacher.MiddleName}".Trim()))
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
            .ForMember(dest => dest.SubjectTypeName, opt => opt.MapFrom(src => src.Subject.SubjectType.Name));
    }
}