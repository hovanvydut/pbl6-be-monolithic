using Monolithic.Models.Common;
using Monolithic.Models.Entities;
namespace Monolithic.Repositories.Interface;

public interface ICategoryRepository
{
    Task<PagedList<CategoryEntity>> GetAllWithFilter(ReqParam reqParam);
    Task<List<CategoryEntity>> GetAllHouseType();
    Task<CategoryEntity> GetHouseTypeById(int id);
}