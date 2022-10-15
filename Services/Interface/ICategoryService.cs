using Monolithic.Models.Common;
using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface ICategoryService
{
    Task<PagedList<CategoryDTO>> GetAllWithFilter(ReqParam reqParam);
    Task<List<CategoryDTO>> GetAllHouseType();
}