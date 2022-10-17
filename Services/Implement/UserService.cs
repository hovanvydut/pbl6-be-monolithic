using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.DTO;
using AutoMapper;
using Monolithic.Models.Entities;

namespace Monolithic.Services.Implement;

public class UserService : IUserService
{
    private readonly IUserAccountReposiory _userAccountRepository;
    private readonly IUserProfileReposiory _userProfileRepository;
    private readonly IMapper _mapper;

    public UserService(IUserAccountReposiory userAccountRepository,
                       IUserProfileReposiory userProfileRepository,
                       IMapper mapper)
    {
        _userAccountRepository = userAccountRepository;
        _userProfileRepository = userProfileRepository;
        _mapper = mapper;
    }

    public async Task<UserProfilePersonalDTO> GetUserProfilePersonal(int userId)
    {
        var user = await _userProfileRepository.GetByUserId(userId);
        var userPersonal = _mapper.Map<UserProfilePersonalDTO>(user);
        // TODO: get address
        return userPersonal;
    }

    public async Task<UserProfileAnonymousDTO> GetUserProfileAnonymous(int userId)
    {
        var user = await _userProfileRepository.GetByUserId(userId);
        var userAnonymous = _mapper.Map<UserProfileAnonymousDTO>(user);
        // TODO: get address
        return userAnonymous;
    }

    public async Task<bool> UpdateUserProfile(int userId, UserProfileUpdateDTO userProfileUpdateDTO)
    {
        var userProfileEntity = _mapper.Map<UserProfileEntity>(userProfileUpdateDTO);
        return await _userProfileRepository.Update(userId, userProfileEntity);
    }
}