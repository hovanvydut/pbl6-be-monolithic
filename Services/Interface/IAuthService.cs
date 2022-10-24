using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IAuthService
{
    Task<UserRegisterResponseDTO> Register(UserRegisterDTO userRegisterDTO, string scheme, string host);

    Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDTO);

    Task<bool> ConfirmEmail(int userId, string code);

    Task<bool> ChangePassword(int userId, UserChangePasswordDTO userChangePasswordDTO);

    Task ForgotPassword(string email, string scheme, string host);

    Task RecoverPassword(UserRecoverPasswordDTO userRecoverPasswordDTO);
}