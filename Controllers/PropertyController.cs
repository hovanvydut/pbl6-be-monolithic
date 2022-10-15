using Microsoft.AspNetCore.Mvc;
using Monolithic.Common;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class PropertyController : BaseController
{
    private readonly IPropertyService _propertyService;

    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPropertyGroup()
    {
        var propertyGroupList = await _propertyService.GetAllPropertyGroup();
        return Ok(propertyGroupList);
    }
}