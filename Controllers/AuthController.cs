using Monolithic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.DTO;

namespace Monolithic.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
    {
        if (ModelState.IsValid)
        {
            var newUser = await _authService.Register(userRegisterDTO);
            if (newUser == null)
            {
                return BadRequest("This email is existing");
            }
            return Ok(newUser);
        }
        return BadRequest();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
    {
        if (ModelState.IsValid)
        {
            var userLogin = await _authService.Login(userLoginDTO);
            if (userLogin == null)
            {
                return Unauthorized("Invalid email or password");
            }
            return Ok(userLogin);
        }
        return BadRequest();
    }
}