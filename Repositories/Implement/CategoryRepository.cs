using Monolithic.Models.Common;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;
using Monolithic.Models.Context;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.ReqParams;

namespace Monolithic.Repositories.Implement;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _db;

    public CategoryRepository(DataContext db)
    {
        _db = db;
    }

    // public async Task<PagedList<CategoryEntity>> GetAllWithFilter(CategoryParams categoryParams)
    // {
    //     // PagedList<CategoryEntity> categoryList = await _db.Categories.Where(c => c.CreatedAt == null)
    //     //                             .ToPagedList(categoryParams.PageNumber, categoryParams.PageSize);
    //     // return categoryList;
    //     return null;
    // }

    public async Task<List<CategoryEntity>> GetAllHouseType()
    {
        return await _db.Categories.ToListAsync();
    }

    public async Task<CategoryEntity> GetHouseTypeById(int id)
    {
        return await _db.Categories.FindAsync(id);
    }
}
