using Monolithic.Models.DTO;
using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;

namespace Monolithic.Services.Implement;

public class PaymentHistoryService : IPaymentHistoryService
{
    private readonly IPaymentHistoryRepository _paymentHistoryRepo;
    public PaymentHistoryService(IPaymentHistoryRepository paymentHistoryRepo)
    {
        _paymentHistoryRepo = paymentHistoryRepo;
    }

    public Task<PaymentHistoryDTO> PayForCreatePost(int hostId, int postId)
    {
        throw new NotImplementedException();
    }
}