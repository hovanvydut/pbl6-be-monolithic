using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using AutoMapper;

namespace Monolithic.Services.Implement;

public class UserService : IUserService
{
    private readonly IUserAccountReposiory _userAccountRepo;
    private readonly IUserProfileReposiory _userProfileRepo;
    private readonly IRoleRepository _roleRepo;
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;

    public UserService(IUserAccountReposiory userAccountRepo,
                       IUserProfileReposiory userProfileRepo,
                       IRoleRepository roleRepo,
                       IAddressService addressService,
                       IMapper mapper)
    {
        _userAccountRepo = userAccountRepo;
        _userProfileRepo = userProfileRepo;
        _roleRepo = roleRepo;
        _addressService = addressService;
        _mapper = mapper;
    }

    public async Task<UserProfilePersonalDTO> GetUserProfilePersonal(int userId)
    {
        var user = await _userProfileRepo.GetByUserId(userId);
        if (user == null)
            throw new BaseException(HttpCode.NOT_FOUND, "This user id = " + userId + " is not found");
        var userPersonal = _mapper.Map<UserProfilePersonalDTO>(user);
        var userAddress = await _addressService.GetAddress(userPersonal.AddressWardId);
        userPersonal.AddressWard = userAddress.ward.Name;
        userPersonal.AddressDistrict = userAddress.district.Name;
        userPersonal.AddressProvince = userAddress.province.Name;
        return userPersonal;
    }

    public async Task<UserProfileAnonymousDTO> GetUserProfileAnonymous(int userId)
    {
        var user = await _userProfileRepo.GetByUserId(userId);
        if (user == null)
            throw new BaseException(HttpCode.NOT_FOUND, "This user id = " + userId + " is not found");
        var userAnonymous = _mapper.Map<UserProfileAnonymousDTO>(user);
        var userAddress = await _addressService.GetAddress(userAnonymous.AddressWardId);
        userAnonymous.AddressWard = userAddress.ward.Name;
        userAnonymous.AddressDistrict = userAddress.district.Name;
        userAnonymous.AddressProvince = userAddress.province.Name;
        return userAnonymous;
    }

    public async Task<UserAccountDTO> GetUserAccountById(int userId)
    {
        var userAccount = await _userAccountRepo.GetById(userId);
        if (userAccount == null)
            throw new BaseException(HttpCode.NOT_FOUND, "This user account id = " + userId + " is not found");
        return _mapper.Map<UserAccountDTO>(userAccount);
    }

    public async Task<bool> UpdateUserProfile(int userId, UserProfileUpdateDTO userProfileUpdateDTO)
    {
        var userProfileEntity = _mapper.Map<UserProfileEntity>(userProfileUpdateDTO);
        return await _userProfileRepo.Update(userId, userProfileEntity);
    }

    public async Task<bool> UpdateUserAccount(int userId, UserAccountUpdateDTO userAccountUpdateDTO)
    {
        var userDB = await _userAccountRepo.GetById(userId);
        if (userDB == null)
            throw new BaseException(HttpCode.NOT_FOUND, "This user is not found");
        userDB.IsActived = userAccountUpdateDTO.IsActived;
        userDB.RoleId = userAccountUpdateDTO.RoleId;
        userDB.Role = null;
        return await _userAccountRepo.Update(userId, userDB);
    }

    public async Task<PagedList<UserDTO>> GetAllUsers(UserParams userParams)
    {
        var listUser = await _userAccountRepo.GetAllUsers(userParams);
        var listUserDTO = listUser.Records.Select(user => new UserDTO()
        {
            AccountId = user.Id,
            ProfileId = user.UserProfile.Id,
            Email = user.Email,
            DisplayName = user.UserProfile.DisplayName,
            PhoneNumber = user.UserProfile.PhoneNumber,
            IdentityNumber = user.UserProfile.IdentityNumber,
            Avatar = user.UserProfile.Avatar,
            Address = user.UserProfile.Address,
            AddressWard = user.UserProfile.AddressWard.Name,
            AddressDistrict = user.UserProfile.AddressWard.AddressDistrict.Name,
            AddressProvince = user.UserProfile.AddressWard.AddressDistrict.AddressProvince.Name
        }).ToList();
        return new PagedList<UserDTO>(listUserDTO, listUser.TotalRecords);
    }

    public async Task<bool> UserMakePayment(int userId, double amount)
    {
        return await _userProfileRepo.UserMakePayment(userId, amount);
    }

    public async Task<List<string>> GetPersonalPermissions(int userId)
    {
        var user = await _userAccountRepo.GetById(userId);
        var permissions = await _roleRepo.GetPermissionByRoleId(user.RoleId);
        return permissions.Select(p => p.Key).ToList();
    }
}