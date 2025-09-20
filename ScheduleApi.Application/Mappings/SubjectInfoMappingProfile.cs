using AutoMapper;
using ScheduleApi.Application.DTOs.SubjectInfo;

namespace ScheduleApi.Application.Mappings;

public class SubjectInfoMappingProfile : Profile
{
    public SubjectInfoMappingProfile()
    {
        CreateMap<CreateSubjectInfoDto, Core.Entities.SubjectInfo>();

        CreateMap<Core.Entities.SubjectInfo, SubjectInfoDto>()
            .ForMember(dest => dest.InfoTypeName,
                opt => opt.MapFrom(src => src.InfoType.Name));
    }
}