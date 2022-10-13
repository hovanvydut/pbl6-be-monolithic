using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IAuthService
{
    Task<UserRegisterResponseDTO> Register(UserRegisterDTO userRegisterDTO);

    Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDTO);
}