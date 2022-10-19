using Monolithic.Services.Interface;
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

    [HttpGet("Personal")]
    public async Task<BaseResponse<UserProfilePersonalDTO>> GetUserProfilePersonal(int userId)
    {
        var userProfile = await _userService.GetUserProfilePersonal(userId);
        return new BaseResponse<UserProfilePersonalDTO>(userProfile, HttpCode.OK);
    }

    [HttpGet("Anonymous")]
    public async Task<BaseResponse<UserProfileAnonymousDTO>> GetUserProfileAnonymous(int userId)
    {
        var userProfile = await _userService.GetUserProfileAnonymous(userId);
        return new BaseResponse<UserProfileAnonymousDTO>(userProfile, HttpCode.OK);
    }

    [HttpPut("{userId}")]
    public async Task<BaseResponse<bool>> UpdateUserProfile(int userId, [FromBody] UserProfileUpdateDTO userProfileUpdateDTO)
    {
        if (ModelState.IsValid)
        {
            var userUpdated = await _userService.UpdateUserProfile(userId, userProfileUpdateDTO);
            if (userUpdated)
                return new BaseResponse<bool>(userUpdated, HttpCode.NO_CONTENT);
            else
                return new BaseResponse<bool>(userUpdated, HttpCode.BAD_REQUEST);
        }
        return new BaseResponse<bool>(false, HttpCode.BAD_REQUEST);
    }
}