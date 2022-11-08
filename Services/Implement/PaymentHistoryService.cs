using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using AutoMapper;

namespace Monolithic.Services.Implement;

public class PaymentHistoryService : IPaymentHistoryService
{
    private readonly IPaymentHistoryRepository _paymentHistoryRepo;
    private readonly IMapper _mapper;
    public PaymentHistoryService(IPaymentHistoryRepository paymentHistoryRepo, IMapper mapper)
    {
        _paymentHistoryRepo = paymentHistoryRepo;
        _mapper = mapper;
    }

    public async Task<PagedList<PaymentHistoryDTO>> GetWithParams(int hostId, PaymentHistoryParams paymentHistoryParams)
    {
        PagedList<PaymentHistoryEntity> historyEntityList = await _paymentHistoryRepo.GetWithParams(hostId, paymentHistoryParams);
        List<PaymentHistoryDTO> historyDTOList = historyEntityList.Records.Select(h => _mapper.Map<PaymentHistoryDTO>(h)).ToList();
        return new PagedList<PaymentHistoryDTO>(historyDTOList, historyEntityList.TotalRecords);
    }

    public async Task<PaymentHistoryDTO> PayForCreatePost(int hostId, int postId, double postPaid)
    {
        var paymentHistory = new PaymentHistoryEntity()
        {
            PaymentCode = Guid.NewGuid().ToString("N").ToUpper(),
            HostId = hostId,
            PostId = postId,
            PaymentType = PaymentType.CREATE_POST,
            Amount = postPaid,
            Description = "Thanh toán cho việc đăng bài viết",
        };
        var paymentHistoryCreated = await _paymentHistoryRepo.Create(paymentHistory);
        return _mapper.Map<PaymentHistoryDTO>(paymentHistoryCreated);
    }

    public async Task<PaymentHistoryDTO> PayForUptopPost(int hostId, int postId, double uptopPaid, int days)
    {
        var paymentHistory = new PaymentHistoryEntity()
        {
            PaymentCode = Guid.NewGuid().ToString("N").ToUpper(),
            HostId = hostId,
            PostId = postId,
            PaymentType = PaymentType.UPTOP_POST,
            Amount = uptopPaid,
            Description = $"Thanh toán cho việc đưa bài viết lên mục ưu tiên trong {days} ngày",
        };
        var paymentHistoryCreated = await _paymentHistoryRepo.Create(paymentHistory);
        return _mapper.Map<PaymentHistoryDTO>(paymentHistoryCreated);
    }
}