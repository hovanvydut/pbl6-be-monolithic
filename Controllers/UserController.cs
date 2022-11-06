using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;

namespace Monolithic.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<BaseResponse<PagedList<UserDTO>>> GetAllUsers([FromQuery] UserParams userParams)
    {
        var users = await _userService.GetAllUsers(userParams);
        return new BaseResponse<PagedList<UserDTO>>(users);
    }

    [HttpGet("Personal")]
    // [Authorize(Roles = UserPermission.ViewPersonal)]
    [Authorize]
    public async Task<BaseResponse<UserProfilePersonalDTO>> GetUserProfilePersonal()
    {
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var userProfile = await _userService.GetUserProfilePersonal(reqUser.Id);
        return new BaseResponse<UserProfilePersonalDTO>(userProfile);
    }

    [HttpGet("Anonymous")]
    // [Authorize(Roles = UserPermission.ViewAnonymous)]
    [Authorize]
    public async Task<BaseResponse<UserProfileAnonymousDTO>> GetUserProfileAnonymous(int userId)
    {
        var userProfile = await _userService.GetUserProfileAnonymous(userId);
        return new BaseResponse<UserProfileAnonymousDTO>(userProfile);
    }

    [HttpPut("Personal")]
    // [Authorize(Roles = UserPermission.UpdateProfile)]
    [Authorize]
    public async Task<BaseResponse<bool>> UpdateUserProfilePersonal([FromBody] UserProfileUpdateDTO userProfileUpdateDTO)
    {
        if (ModelState.IsValid)
        {
            ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
            var userUpdated = await _userService.UpdateUserProfile(reqUser.Id, userProfileUpdateDTO);
            if (userUpdated)
                return new BaseResponse<bool>(userUpdated, HttpCode.NO_CONTENT);
            else
                return new BaseResponse<bool>(userUpdated, HttpCode.BAD_REQUEST, "", false);
        }
        return new BaseResponse<bool>(false, HttpCode.BAD_REQUEST, "", false);
    }

    [HttpPut("Account/{userId}")]
    [Authorize]
    public async Task<BaseResponse<bool>> UpdateUserAccount(int userId, [FromBody] UserAccountUpdateDTO userAccountUpdateDTO)
    {
        if (ModelState.IsValid)
        {
            var accountUpdated = await _userService.UpdateUserAccount(userId, userAccountUpdateDTO);
            if (accountUpdated)
                return new BaseResponse<bool>(accountUpdated, HttpCode.NO_CONTENT);
            else
                return new BaseResponse<bool>(accountUpdated, HttpCode.BAD_REQUEST, "", false);
        }
        return new BaseResponse<bool>(false, HttpCode.BAD_REQUEST, "", false);
    }
}