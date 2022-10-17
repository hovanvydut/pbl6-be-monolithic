using Monolithic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.DTO;

namespace Monolithic.Controllers;

public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("Personal")]
    public async Task<IActionResult> GetUserProfilePersonal(int userId)
    {
        var userProfile = await _userService.GetUserProfilePersonal(userId);
        if (userProfile == null)
            return NotFound();
        return Ok(userProfile);
    }

    [HttpGet("Anonymous")]
    public async Task<IActionResult> GetUserProfileAnonymous(int userId)
    {
        var userProfile = await _userService.GetUserProfileAnonymous(userId);
        if (userProfile == null)
            return NotFound();
        return Ok(userProfile);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUserProfile(int userId, [FromBody] UserProfileUpdateDTO userProfileUpdateDTO)
    {
        if (ModelState.IsValid && await _userService.UpdateUserProfile(userId, userProfileUpdateDTO))
        {
            return NoContent();
        }
        return BadRequest();
    }
}