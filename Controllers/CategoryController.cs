using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        this._categoryService = categoryService;
    }


    // [HttpGet]
    // public async Task<IActionResult> GetAllWithFilter([FromQuery] ReqParam reqParam)
    // {
    //     var result = await _categoryService.GetAllWithFilter(reqParam);
    //     return Ok(result);
    // }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _categoryService.GetAll();
        return Ok(result);
    }
}