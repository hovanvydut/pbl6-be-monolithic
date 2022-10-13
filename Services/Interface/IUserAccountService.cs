using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IUserAccountService
{
    Task<UserRegisterResponseDTO> Register(UserRegisterDTO userRegisterDTO);
}