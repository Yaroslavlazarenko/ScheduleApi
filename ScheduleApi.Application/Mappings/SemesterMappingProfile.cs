using AutoMapper;
using ScheduleApi.Application.DTOs.Semester;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class SemesterMappingProfile : Profile
{
    public SemesterMappingProfile()
    {
        CreateMap<CreateSemesterDto, Semester>();
        CreateMap<Semester, SemesterDto>();
    }
}