using AutoMapper;
using ScheduleApi.Application.DTOs.SubjectType;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class SubjectTypeMappingProfile : Profile
{
    public SubjectTypeMappingProfile()
    {
        CreateMap<CreateSubjectTypeDto, SubjectType>();
        CreateMap<SubjectType, SubjectTypeDto>();
    }
}