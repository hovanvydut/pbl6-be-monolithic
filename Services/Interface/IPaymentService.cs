using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IPaymentService
{
    Task<List<BankCodeDTO>> GetAllBankCode();
}