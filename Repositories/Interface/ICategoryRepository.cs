using Monolithic.Models.Common;
using Monolithic.Models.Entities;
using Monolithic.Models.ReqParams;

namespace Monolithic.Repositories.Interface;

public interface ICategoryRepository
{
    Task<PagedList<CategoryEntity>> GetAllWithFilter(CategoryParams categoryParams);
    Task<List<CategoryEntity>> GetAllHouseType();
    Task<CategoryEntity> GetHouseTypeById(int id);
}