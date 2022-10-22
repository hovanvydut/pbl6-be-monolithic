using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IBankCodeRepository
{
    Task<List<BankCodeEntity>> GetAll();
}