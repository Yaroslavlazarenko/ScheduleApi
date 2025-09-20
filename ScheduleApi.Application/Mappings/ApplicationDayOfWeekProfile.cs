using AutoMapper;
using ScheduleApi.Application.DTOs.DayOfWeek;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class ApplicationDayOfWeekProfile : Profile
{
    public ApplicationDayOfWeekProfile()
    {
        CreateMap<CreateDayOfWeekDto, ApplicationDayOfWeek>();
        CreateMap<ApplicationDayOfWeek, DayOfWeekDto>();
    }
}