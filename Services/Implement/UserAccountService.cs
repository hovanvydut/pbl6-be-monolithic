using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using System.Security.Cryptography;
using Monolithic.Models.Entities;
using Monolithic.Models.DTO;
using System.Text;
using AutoMapper;

namespace Monolithic.Services.Implement;

public class UserAccountService : IUserAccountService
{
    private readonly IUserAccountReposiory _userAccountRepository;
    private readonly IMapper _mapper;

    public UserAccountService(IUserAccountReposiory userAccountRepository, IMapper mapper)
    {
        _userAccountRepository = userAccountRepository;
        _mapper = mapper;
    }

    public async Task<bool> IsExistingEmail(string email)
    {
        if (await _userAccountRepository.GetByEmail(email) == null)
        {
            return false;
        }
        return true;
    }

    public async Task<UserRegisterResponseDTO> Register(UserRegisterDTO userRegisterDTO)
    {
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
}