using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Services.Interface;
using Monolithic.Repositories.Interface;
using Monolithic.Models.Entities;
using AutoMapper;
using Monolithic.Models.ReqParams;

namespace Monolithic.Services.Implement;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepo;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepo, IMapper mapper)
    {
        _categoryRepo = categoryRepo;
        _mapper = mapper;
    }

    // public async Task<PagedList<CategoryDTO>> GetAllWithFilter(CategoryParams categoryParams)
    // {
    //     PagedList<CategoryEntity> categoryList = await _categoryRepo.GetAllWithFilter(categoryParams);
    //     PagedList<CategoryDTO> categoryDTOList = new PagedList<CategoryDTO>()
    //     {
    //         Records = categoryList.Select(c => _mapper.Map<CategoryDTO>(c)).ToList(),
    //         TotalRecords = categoryList.TotalRecords
    //     };
    //     return categoryDTOList;
    // }

    public async Task<List<CategoryDTO>> GetAllHouseType()
    {
        List<CategoryEntity> categoryList = await _categoryRepo.GetAllHouseType();
        return categoryList.Select(c => _mapper.Map<CategoryDTO>(c)).ToList();
    }
}