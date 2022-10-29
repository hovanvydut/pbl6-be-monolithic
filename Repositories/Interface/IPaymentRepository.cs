using Monolithic.Models.DTO;
using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IPaymentRepository
{
    Task<List<BankCodeEntity>> GetAllBankCode();
    Task CreateVNPHistory(int userId, VNPHistoryDTO vnpHistoryDTO);
    Task<VNPHistoryEntity> GetByTxnRef(long txtRef);
    Task updateStatusTransaction(long txtRef, string status);
}