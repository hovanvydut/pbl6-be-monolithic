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
            var scheme = HttpContext.Request.Scheme;
            var host = HttpContext.Request.Host.Value;
            var newUser = await _authService.Register(userRegisterDTO, scheme, host);
            if (newUser == null)
            {
                return BadRequest("Email, phone number or identity number already exists");
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

    [HttpGet("Confirm-Email")]
    public async Task<IActionResult> ConfirmEmail(int userId)
    {
        if (ModelState.IsValid)
        {
            var userConfirm = await _authService.ConfirmEmail(userId);
            if (!userConfirm)
            {
                return BadRequest("Email confirm request is invalid");
            }
            return Ok();
        }
        return BadRequest();
    }
}