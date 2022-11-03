using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;

namespace Monolithic.Repositories.Interface;

public interface IPaymentHistoryRepository
{
    Task<PagedList<PaymentHistoryEntity>> GetWithParams(int hostId, PaymentHistoryParams paymentHistoryParams);

    Task<PaymentHistoryEntity> GetById(int id);

    Task<PaymentHistoryEntity> Create(PaymentHistoryEntity paymentHistoryEntity);
}