using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;

namespace Monolithic.Services.Interface;

public interface ICategoryService
{
    // Task<PagedList<CategoryDTO>> GetAllWithFilter(CategoryParams categoryParams);
    Task<List<CategoryDTO>> GetAllHouseType();
}