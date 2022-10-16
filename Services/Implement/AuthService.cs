using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using System.Security.Cryptography;
using Monolithic.Models.Entities;
using Monolithic.Models.DTO;
using System.Text;
using AutoMapper;

namespace Monolithic.Services.Implement;

public class AuthService : IAuthService
{
    private readonly IUserAccountReposiory _userAccountRepository;
    private readonly IUserProfileReposiory _userProfileReposiory;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(IUserAccountReposiory userAccountRepository,
                       IUserProfileReposiory userProfileReposiory,
                       ITokenService tokenService,
                       IMapper mapper)
    {
        _userAccountRepository = userAccountRepository;
        _userProfileReposiory = userProfileReposiory;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<UserRegisterResponseDTO> Register(UserRegisterDTO userRegisterDTO)
    {
        // Return null if register email is exists
        if (await _userAccountRepository.GetByEmail(userRegisterDTO.Email) != null)
            return null;
        using var hmac = new HMACSHA512();
        var passwordByte = Encoding.UTF8.GetBytes(userRegisterDTO.Password);

        // Create user account
        var newUserAccount = _mapper.Map<UserAccountEntity>(userRegisterDTO);
        newUserAccount.PasswordSalt = hmac.Key;
        newUserAccount.PasswordHashed = hmac.ComputeHash(passwordByte);
        newUserAccount.IsVerified = true;
        await _userAccountRepository.Create(newUserAccount);

        // Create user profile with new user account Id
        var newUserProfile = _mapper.Map<UserProfileEntity>(userRegisterDTO);
        newUserProfile.CurrentCredit = 0;
        newUserProfile.UserAccountId = newUserAccount.Id;
        await _userProfileReposiory.Create(newUserProfile);

        return _mapper.Map<UserRegisterResponseDTO>(newUserAccount);
    }

    public async Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDTO)
    {
        var currentUser = await _userAccountRepository.GetByEmail(userLoginDTO.Email);
        if (currentUser == null || !currentUser.IsVerified)
            return null;

        using var hmac = new HMACSHA512(currentUser.PasswordSalt);
        var passwordByte = Encoding.UTF8.GetBytes(userLoginDTO.Password);
        var computedHash = hmac.ComputeHash(passwordByte);
        if (!computedHash.SequenceEqual(currentUser.PasswordHashed))
            return null;

        return new UserLoginResponseDTO()
        {
            Id = currentUser.Id,
            Email = currentUser.Email,
            AccessToken = _tokenService.CreateToken(currentUser)
        };
    }
}