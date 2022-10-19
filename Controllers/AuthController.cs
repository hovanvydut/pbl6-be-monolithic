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
            var scheme = HttpContext.Request.Scheme;
            var host = HttpContext.Request.Host.Value;
            var newUser = await _authService.Register(userRegisterDTO, scheme, host);
            return new BaseResponse<UserRegisterResponseDTO>(newUser, HttpCode.CREATED);
        }
        return new BaseResponse<UserRegisterResponseDTO>(null, HttpCode.BAD_REQUEST);
    }

    [HttpPost("Login")]
    public async Task<BaseResponse<UserLoginResponseDTO>> Login([FromBody] UserLoginDTO userLoginDTO)
    {
        if (ModelState.IsValid)
        {
            var userLogin = await _authService.Login(userLoginDTO);
            return new BaseResponse<UserLoginResponseDTO>(userLogin, HttpCode.OK);
        }
        return new BaseResponse<UserLoginResponseDTO>(null, HttpCode.BAD_REQUEST);
    }

    [HttpGet("Confirm-Email")]
    public async Task<BaseResponse<bool>> ConfirmEmail(int userId)
    {
        var userConfirm = await _authService.ConfirmEmail(userId);
        return new BaseResponse<bool>(userConfirm, HttpCode.OK);
    }
}