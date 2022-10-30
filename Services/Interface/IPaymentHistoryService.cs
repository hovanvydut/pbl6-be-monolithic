using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IPaymentHistoryService
{
    Task<PaymentHistoryDTO> PayForCreatePost(int hostId, int postId);
}