using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IAuthService
{
    Task<UserRegisterResponseDTO> Register(UserRegisterDTO userRegisterDTO);

    Task<UserLoginResponseDTO> Login(UserLoginDTO userLoginDTO);

    Task<bool> ConfirmEmail(UserConfirmEmailDTO userConfirmEmailDTO);

    Task<bool> ChangePassword(int userId, UserChangePasswordDTO userChangePasswordDTO);

    Task ForgotPassword(string email);

    Task RecoverPassword(UserRecoverPasswordDTO userRecoverPasswordDTO);
}