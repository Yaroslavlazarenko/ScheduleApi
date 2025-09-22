using AutoMapper;
using ScheduleApi.Application.DTOs.SubjectInfo;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class SubjectInfoMappingProfile : Profile
{
    public SubjectInfoMappingProfile()
    {
        CreateMap<CreateSubjectInfoDto, SubjectInfo>();

        CreateMap<SubjectInfo, SubjectInfoDto>()
            .ForMember(dest => dest.InfoTypeName,
                opt => opt.MapFrom(src => src.InfoType.Name));
    }
}