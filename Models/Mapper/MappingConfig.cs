using Monolithic.Models.Entities;
using Monolithic.Models.DTO;
using Monolithic.Helpers;
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
        CreateMap<UserAccountEntity, UserAccountDTO>()
            .ForMember(dto => dto.RoleName, prop => prop.MapFrom(entity => entity.Role.Name));

        // User manager
        CreateMap<UserProfileEntity, UserProfilePersonalDTO>()
            .ForMember(dto => dto.UserAccountEmail, prop => prop.MapFrom(entity => entity.UserAccount.Email));
        CreateMap<UserProfileEntity, UserProfileAnonymousDTO>();
        CreateMap<UserProfileUpdateDTO, UserProfileEntity>();

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
            .ForMember(dest => dest.PropertyGroupId, act => act.MapFrom(src => src.Property.PropertyGroupId))
            .PreserveReferences();
        CreateMap<PostEntity, PostDTO>()
            .ForMember(dest => dest.Properties, act => act.MapFrom(src => src.PostProperties))
            .ForMember(dest => dest.FullAddress, act => act.MapFrom(src => src.AddressWard))
            .ForMember(dest => dest.Address, act => act.MapFrom(src => src.Address))
            .ForMember(dto => dto.AuthorInfo, act => act.MapFrom(src => src.HostAccount.UserProfile))
            .PreserveReferences();

        // media
        CreateMap<CreateMediaDTO, MediaEntity>();
        CreateMap<MediaEntity, MediaDTO>();

        // role - permission
        CreateMap<CreateRoleDTO, RoleEntity>();
        CreateMap<UpdateRoleDTO, RoleEntity>();
        CreateMap<CreatePermissionDTO, PermissionEntity>();
        CreateMap<PermissionEntity, PermissionDTO>();

        // bookmark
        CreateMap<BookmarkEntity, BookmarkDTO>()
            .ForMember(dto => dto.Id, prop => prop.MapFrom(entity => entity.Post.Id))
            .ForMember(dto => dto.Title, prop => prop.MapFrom(entity => entity.Post.Title))
            .ForMember(dto => dto.FullAddress, prop => prop.MapFrom(entity => entity.Post.AddressWard))
            .ForMember(dto => dto.Price, prop => prop.MapFrom(entity => entity.Post.Price))
            .ForMember(dto => dto.Slug, prop => prop.MapFrom(entity => entity.Post.Slug))
            .ForMember(dto => dto.Address, prop => prop.MapFrom(entity => entity.Post.Address))
            .ForMember(dto => dto.Area, prop => prop.MapFrom(entity => entity.Post.Area))
            .ForMember(dto => dto.Category, prop => prop.MapFrom(entity => entity.Post.Category))
            .PreserveReferences();
        // bank code
        CreateMap<BankCodeEntity, BankCodeDTO>();
        CreateMap<VNPHistoryDTO, VNPHistoryEntity>();
        CreateMap<VNPHistoryEntity, UserVNPHistoryDTO>()
            .ForMember(dto => dto.OrderInfo, prop => prop.MapFrom(entity => entity.vnp_OrderInfo))
            .ForMember(dto => dto.Amount, prop => prop.MapFrom(entity => entity.vnp_Amount))
            .ForMember(dto => dto.BankCode, prop => prop.MapFrom(entity => entity.vnp_BankCode))
            .ForMember(dto => dto.UserEmail, prop => prop.MapFrom(entity => entity.UserAccount.Email))
            .ForMember(dto => dto.TransactionStatus, prop => prop.MapFrom(entity =>
                                                VNPStatus.GetVNPStatus(entity.vnp_TransactionStatus)));

        // config setting
        CreateMap<ConfigSettingEntity, ConfigSettingDTO>();
        CreateMap<ConfigSettingUpdateDTO, ConfigSettingEntity>();

        // payment history
        CreateMap<PaymentHistoryEntity, PaymentHistoryDTO>()
            .ForMember(dto => dto.HostEmail, prop => prop.MapFrom(entity => entity.HostAccount.Email));
        // booking
        CreateMap<FreeTimeDTO, FreeTimeEntity>();
        CreateMap<FreeTimeEntity, FreeTimeDTO>();
        CreateMap<CreateMeetingDTO, MeetingEntity>();
        CreateMap<UserProfileEntity, GuestMeetingDTO>();
        CreateMap<MeetingEntity, MeetingDTO>()
            .ForMember(dto => dto.GuestInfo, prop => prop.MapFrom(entity => entity.GuestAccount.UserProfile));

        // review
        CreateMap<CreateReviewDTO, ReviewEntity>();
        CreateMap<ReviewEntity, ReviewDTO>()
            .ForMember(dto => dto.UserInfo, prop => prop.MapFrom(e => e.UserAccount.UserProfile));

        // priority post
        CreateMap<PriorityPostCreateDTO, PriorityPostEntity>();
        CreateMap<PriorityPostEntity, PriorityPostDTO>()
            .ForMember(dto => dto.Id, prop => prop.MapFrom(entity => entity.Post.Id))
            .ForMember(dto => dto.Title, prop => prop.MapFrom(entity => entity.Post.Title))
            .ForMember(dto => dto.Slug, prop => prop.MapFrom(entity => entity.Post.Slug))
            .ForMember(dto => dto.Address, prop => prop.MapFrom(entity => entity.Post.Address));

        // statistic
        CreateMap<PostStatisticEntity, PostStatisticDTO>()
            .ForMember(dto => dto.Id, prop => prop.MapFrom(entity => entity.PostId))
            .ForMember(dto => dto.Title, prop => prop.MapFrom(entity => entity.Post.Title))
            .ForMember(dto => dto.Slug, prop => prop.MapFrom(entity => entity.Post.Slug))
            .ForMember(dto => dto.IsDeleted, prop => prop.MapFrom(entity => entity.Post.DeletedAt != null))
            .ForMember(dto => dto.StatisticValue, prop => prop.MapFrom(entity => entity.Value));

        CreateMap<UserStatisticEntity, UserStatisticDTO>()
            .ForMember(dto => dto.Id, prop => prop.MapFrom(entity => entity.UserId))
            .ForMember(dto => dto.Email, prop => prop.MapFrom(entity => entity.UserAccount.Email))
            .ForMember(dto => dto.StatisticValue, prop => prop.MapFrom(entity => entity.Value));
    }
}