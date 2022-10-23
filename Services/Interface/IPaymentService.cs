using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IPaymentService
{
    Task<List<BankCodeDTO>> GetAllBankCode();
    Task<string> CreatePayement(CreatePaymentDTO createPaymentDTO);
    Task ReceiveDataFromVNP(VNPayReturnDTO vnpayReturnDTO);
}