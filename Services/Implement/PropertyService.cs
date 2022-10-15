using AutoMapper;
using Monolithic.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;

namespace Monolithic.Services.Implement;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepo;
    private readonly IMapper _mapper; 

    public PropertyService(IPropertyRepository propertyRepo, IMapper mapper)
    {
        _propertyRepo = propertyRepo;
        _mapper = mapper;
    }

    public async Task<List<PropertyGroupDTO>> GetAllPropertyGroup()
    {
        List<PropertyGroupEntity> groupEntityList = await _propertyRepo.GetAllPropertiesGroup();
        var result = groupEntityList.Select(g => _mapper.Map<PropertyGroupDTO>(g)).ToList();
        return result;
    }
}