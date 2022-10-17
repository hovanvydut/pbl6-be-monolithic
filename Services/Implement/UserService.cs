using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using AutoMapper;

namespace Monolithic.Services.Implement;

public class UserService : IUserService
{
    private readonly IUserAccountReposiory _userAccountRepository;
    private readonly IUserProfileReposiory _userProfileRepository;
    private readonly IAddressService _addressService;
    private readonly IMapper _mapper;

    public UserService(IUserAccountReposiory userAccountRepository,
                       IUserProfileReposiory userProfileRepository,
                       IAddressService addressService,
                       IMapper mapper)
    {
        _userAccountRepository = userAccountRepository;
        _userProfileRepository = userProfileRepository;
        _addressService = addressService;
        _mapper = mapper;
    }

    public async Task<UserProfilePersonalDTO> GetUserProfilePersonal(int userId)
    {
        var user = await _userProfileRepository.GetByUserId(userId);
        if (user == null)
            throw new BaseException(HttpCode.NOT_FOUND, "This user is not found");
        var userPersonal = _mapper.Map<UserProfilePersonalDTO>(user);
        var userAddress = await _addressService.GetAddress(userPersonal.AddressWardId);
        userPersonal.AddressWard = userAddress.ward.Name;
        userPersonal.AddressDistrict = userAddress.district.Name;
        userPersonal.AddressProvince = userAddress.province.Name;
        return userPersonal;
    }

    public async Task<UserProfileAnonymousDTO> GetUserProfileAnonymous(int userId)
    {
        var user = await _userProfileRepository.GetByUserId(userId);
        if (user == null)
            throw new BaseException(HttpCode.NOT_FOUND, "This user is not found");
        var userAnonymous = _mapper.Map<UserProfileAnonymousDTO>(user);
        var userAddress = await _addressService.GetAddress(userAnonymous.AddressWardId);
        userAnonymous.AddressWard = userAddress.ward.Name;
        userAnonymous.AddressDistrict = userAddress.district.Name;
        userAnonymous.AddressProvince = userAddress.province.Name;
        return userAnonymous;
    }

    public async Task<bool> UpdateUserProfile(int userId, UserProfileUpdateDTO userProfileUpdateDTO)
    {
        var userProfileEntity = _mapper.Map<UserProfileEntity>(userProfileUpdateDTO);
        return await _userProfileRepository.Update(userId, userProfileEntity);
    }
}