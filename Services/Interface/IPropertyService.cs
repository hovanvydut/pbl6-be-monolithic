using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IPropertyService
{
    Task<List<PropertyGroupDTO>> GetAllPropertyGroup();
}