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
        CreateMap<AddressWardEntity, WardDTO>().PreserveReferences();
        CreateMap<AddressDistrictEntity, DistrictDTO>().PreserveReferences();
        CreateMap<AddressProvinceEntity, ProvinceDTO>().PreserveReferences();
        CreateMap<AddressWardEntity, AddressDTO>().PreserveReferences();
            
        // post
        CreateMap<CreatePostDTO, PostEntity>().PreserveReferences();

        CreateMap<PostPropertyEntity, PropertyDTO>()
            .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Property.Id))
            .ForMember(dest => dest.DisplayName, act => act.MapFrom(src => src.Property.DisplayName))
            .PreserveReferences();
        CreateMap<PostEntity, PostDTO>()
            .ForMember(dest => dest.Properties, act => act.MapFrom(src => src.PostProperties))
            .PreserveReferences();

        // media
        CreateMap<CreateMediaDTO, MediaEntity>();
        CreateMap<MediaEntity, MediaDTO>();
    }
}