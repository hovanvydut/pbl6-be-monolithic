using Monolithic.Models.Entities;

namespace Monolithic.Services.Interface;

public interface ITokenService
{
    Task<string> CreateToken(UserAccountEntity user);
}