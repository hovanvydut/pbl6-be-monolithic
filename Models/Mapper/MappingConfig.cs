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

        // Category
        CreateMap<CategoryEntity, CategoryDTO>();

        // User Register
        CreateMap<UserAccountEntity, UserRegisterResponseDTO>()
            .ForMember(dto => dto.RoleName, prop => prop.MapFrom(entity => entity.Role.Name));
        CreateMap<UserRegisterDTO, UserAccountEntity>();
        CreateMap<UserRegisterDTO, UserProfileEntity>();

        // Property, PropertyGroup
        CreateMap<PropertyGroupEntity, PropertyGroupDTO>();
        CreateMap<PropertyEntity, PropertyDTO>();

        // Address
        CreateMap<AddressWardEntity, WardDTO>().PreserveReferences();
            // .ForMember(dest => dest.AddressDistrict, act => act.MapFrom(src => src.AddressDistrict));
        CreateMap<AddressDistrictEntity, DistrictDTO>().PreserveReferences();
        CreateMap<AddressProvinceEntity, ProvinceDTO>().PreserveReferences();
        CreateMap<AddressWardEntity, AddressDTO>()
        //     .ForMember(dest => dest.province, act => act.MapFrom(src => src.AddressDistrict.AddressProvince))
        //     .ForMember(dest => dest.district, act => act.MapFrom(src => src.AddressDistrict))
            // .ForMember(dest => dest.ward, act => act.MapFrom(src => src))
            .PreserveReferences();
    }
}