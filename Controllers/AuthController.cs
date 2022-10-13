using Monolithic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.DTO;

namespace Monolithic.Controllers;

public class AuthController : BaseController
{
    private readonly IUserAccountService _userAccountService;

    public AuthController(IUserAccountService userAccountService)
    {
        _userAccountService = userAccountService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
    {
        if (ModelState.IsValid)
        {
            var newUser = await _userAccountService.Register(userRegisterDTO);
            if (newUser == null)
            {
                return BadRequest("This email is existing");
            }
            return Ok(newUser);
        }
        return BadRequest();
    }
}