using Monolithic.Models.Entities;
using Monolithic.Models.DTO;
using AutoMapper;

namespace Monolithic.Models.Mapper;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
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
        CreateMap<AddressWardEntity, AddressDTO>();
        CreateMap<AddressDistrictEntity, AddressDTO>();
        CreateMap<AddressProvinceEntity, AddressDTO>();

        CreateMap<AddressWardEntity, WardDTO>().PreserveReferences();
        CreateMap<AddressDistrictEntity, DistrictDTO>().PreserveReferences();
        CreateMap<AddressProvinceEntity, ProvinceDTO>().PreserveReferences();
        CreateMap<AddressWardEntity, FullAddressDTO>()
            .ForMember(dest => dest.ward, act => act.MapFrom(src => src))
            .ForMember(dest => dest.district, act => act.MapFrom(src => src.AddressDistrict))
            .ForMember(dest => dest.province, act => act.MapFrom(src => src.AddressDistrict.AddressProvince))
            .PreserveReferences();
            
        // post
        CreateMap<CreatePostDTO, PostEntity>().PreserveReferences();
        CreateMap<UpdatePostDTO, PostEntity>().PreserveReferences();

        CreateMap<PostPropertyEntity, PropertyDTO>()
            .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Property.Id))
            .ForMember(dest => dest.DisplayName, act => act.MapFrom(src => src.Property.DisplayName))
            .PreserveReferences();
        CreateMap<PostEntity, PostDTO>()
            .ForMember(dest => dest.Properties, act => act.MapFrom(src => src.PostProperties))
            .ForMember(dest => dest.Address, act => act.MapFrom(src => src.AddressWard))
            .PreserveReferences();

        // media
        CreateMap<CreateMediaDTO, MediaEntity>();
        CreateMap<MediaEntity, MediaDTO>();
    }
}