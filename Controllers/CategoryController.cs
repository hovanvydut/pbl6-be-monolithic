using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        this._categoryService = categoryService;
    }

    [HttpGet("house-type")]
    public async Task<ActionResult<List<CategoryDTO>>> GetAllHouseType()
    {
        var result = await _categoryService.GetAllHouseType();
        return Ok(result);
    }
}