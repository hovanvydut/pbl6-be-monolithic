using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IPaymentHistoryRepository
{
    Task<List<PaymentHistoryEntity>> GetAllByHostId(int hostid);

    Task<PaymentHistoryEntity> GetById(int id);

    Task<PaymentHistoryEntity> Create(PaymentHistoryEntity paymentHistoryEntity);
}