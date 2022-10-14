using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IPropertyRepository
{
    Task<List<PropertyEntity>> GetAllProperties();
    Task<List<PropertyGroupEntity>> GetAllPropertiesGroup();
}