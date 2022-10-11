using Monolithic.Services.Interface;
using System.Security.Cryptography;
using Monolithic.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.DTO;
using System.Text;

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
            if (await _userAccountService.IsExistingEmail(userRegisterDTO.Email))
            {
                return BadRequest("This email is existing");
            }
            var newUser = await _userAccountService.Register(userRegisterDTO);
            return Ok(newUser);
        }
        return BadRequest();
    }
}