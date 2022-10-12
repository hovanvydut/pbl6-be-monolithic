using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class CommonController : BaseController
{
    private IConfiguration _config;

    public CommonController(IConfiguration configuration)
    {
        _config = configuration;
    }

    [HttpGet("Version")]
    public IActionResult GetAPIVersion()
    {
        return Ok(new {Version = _config["APIVersion"]});
    }
}