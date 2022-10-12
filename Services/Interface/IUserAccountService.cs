using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IUserAccountService
{
    Task<bool> IsExistingEmail(string email);

    Task<UserRegisterResponseDTO> Register(UserRegisterDTO userRegisterDTO);
}