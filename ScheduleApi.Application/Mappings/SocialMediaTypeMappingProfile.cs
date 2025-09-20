using AutoMapper;
using ScheduleApi.Application.DTOs.SocialMediaType;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class SocialMediaTypeMappingProfile : Profile
{
    public SocialMediaTypeMappingProfile()
    {
        CreateMap<CreateSocialMediaTypeDto, SocialMediaType>();
        CreateMap<SocialMediaType, SocialMediaTypeDto>();
    }
}