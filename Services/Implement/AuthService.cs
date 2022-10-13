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
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(IUserAccountReposiory userAccountRepository,
                       ITokenService tokenService,
                       IMapper mapper)
    {
        _userAccountRepository = userAccountRepository;
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
        var newUser = new UserAccountEntity()
        {
            Email = userRegisterDTO.Email,
            PasswordSalt = hmac.Key,
            PasswordHashed = hmac.ComputeHash(passwordByte),
            IsVerified = true,
            RoleId = userRegisterDTO.RoleId,
        };
        await _userAccountRepository.Create(newUser);
        return _mapper.Map<UserRegisterResponseDTO>(newUser);
    }

    public async Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDTO)
    {
        var currentUser = await _userAccountRepository.GetByEmail(userLoginDTO.Email);
        if (currentUser == null)
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