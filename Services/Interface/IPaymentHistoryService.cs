using Monolithic.Models.ReqParams;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IPaymentHistoryService
{
    Task<PagedList<PaymentHistoryDTO>> GetWithParams(int hostId, PaymentHistoryParams paymentHistoryParams);

    Task<PaymentHistoryDTO> PayForCreatePost(int hostId, int postId, double postPaid);

    Task<PaymentHistoryDTO> PayForUptopPost(int hostId, int postId, double uptopPaid, int days);
}