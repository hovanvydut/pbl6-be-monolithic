using Monolithic.Models.Entities;

namespace Monolithic.Services.Interface;

public interface ITokenService
{
    string CreateToken(UserAccountEntity user);
}