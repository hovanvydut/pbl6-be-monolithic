using Monolithic.Models.Entities;
using Monolithic.Models.DTO;
using AutoMapper;

namespace Monolithic.Models.Mapper;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        // Mapping for Example
        CreateMap<Example, ExampleDTO>();
    }
}