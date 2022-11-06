using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;

namespace Monolithic.Services.Interface;

public interface IPaymentService
{
    Task<List<BankCodeDTO>> GetAllBankCode();
    Task<string> CreatePayement(int userId, CreatePaymentDTO createPaymentDTO);
    Task ReceiveDataFromVNP(VNPayReturnDTO vnpayReturnDTO);
    Task<PagedList<UserVNPHistoryDTO>> GetVNPHistories(int userId, VNPParams vnpParams);
}