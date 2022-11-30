using Microsoft.AspNetCore.Authorization;
using Monolithic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;

namespace Monolithic.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<BaseResponse<UserRegisterResponseDTO>> Register([FromBody] UserRegisterDTO userRegisterDTO)
    {
        if (ModelState.IsValid)
        {
            var newUser = await _authService.Register(userRegisterDTO);
            return new BaseResponse<UserRegisterResponseDTO>(newUser, HttpCode.CREATED);
        }
        return new BaseResponse<UserRegisterResponseDTO>(null, HttpCode.BAD_REQUEST, "", false);
    }

    [HttpPost("Login")]
    public async Task<BaseResponse<UserLoginResponseDTO>> Login([FromBody] UserLoginDTO userLoginDTO)
    {
        if (ModelState.IsValid)
        {
            var userLogin = await _authService.Login(userLoginDTO);
            return new BaseResponse<UserLoginResponseDTO>(userLogin);
        }
        return new BaseResponse<UserLoginResponseDTO>(null, HttpCode.BAD_REQUEST, "", false);
    }

    [HttpPut("Confirm-Email")]
    public async Task<BaseResponse<bool>> ConfirmEmail(UserConfirmEmailDTO userConfirmEmailDTO)
    {
        var userConfirm = await _authService.ConfirmEmail(userConfirmEmailDTO);
        return new BaseResponse<bool>(userConfirm);
    }

    [HttpPut("Change-Password")]
    [Authorize]
    public async Task<BaseResponse<bool>> ChangePassword(UserChangePasswordDTO userChangePasswordDTO)
    {
        if (ModelState.IsValid)
        {
            ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
            var passwordChanged = await _authService.ChangePassword(reqUser.Id, userChangePasswordDTO);
            if (passwordChanged)
                return new BaseResponse<bool>(passwordChanged, HttpCode.NO_CONTENT, "Change password success");
            else
                return new BaseResponse<bool>(passwordChanged, HttpCode.BAD_REQUEST, "", false);
        }
        return new BaseResponse<bool>(false, HttpCode.BAD_REQUEST, "", false);
    }

    [HttpGet("Forgot-Password")]
    public async Task<BaseResponse<string>> ForgotPassword(string email)
    {
        await _authService.ForgotPassword(email);
        return new BaseResponse<string>("", HttpCode.OK, "Sent email to recover password");
    }

    [HttpPut("Recover-Password")]
    public async Task<BaseResponse<string>> RecoverPassword(UserRecoverPasswordDTO userRecoverPasswordDTO)
    {
        await _authService.RecoverPassword(userRecoverPasswordDTO);
        return new BaseResponse<string>("", HttpCode.OK, "Recover password successfully");
    }
}