using AutoMapper;
using ScheduleApi.Application.DTOs.Pair;
using ScheduleApi.Core.Entities;

namespace ScheduleApi.Application.Mappings;

public class PairMappingProfile : Profile
{
    public PairMappingProfile()
    {
        CreateMap<CreatePairDto, Pair>();
        CreateMap<Pair, PairDto>();
    }
}