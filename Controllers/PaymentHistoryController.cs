using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Constants;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class PaymentHistoryController : BaseController
{
    private readonly IPaymentHistoryService _paymentHistoryService;
    private readonly IMapper _mapper;

    public PaymentHistoryController(IPaymentHistoryService paymentHistoryService,
                                    IMapper mapper)
    {
        _paymentHistoryService = paymentHistoryService;
        _mapper = mapper;
    }

    [HttpGet("/api/payment-history")]
    [Authorize]
    public async Task<BaseResponse<PagedList<PaymentHistoryDTO>>> GetWithParams([FromQuery] PaymentHistoryParams paymentHistoryParams)
    {
        var histories = await _paymentHistoryService.GetWithParams(0, paymentHistoryParams);
        return new BaseResponse<PagedList<PaymentHistoryDTO>>(histories);
    }

    [HttpGet("/api/payment-history/personal")]
    [Authorize]
    public async Task<BaseResponse<PagedList<PaymentHistoryDTO>>> GetWithParamsPersonal([FromQuery] PaymentHistoryParams paymentHistoryParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var histories = await _paymentHistoryService.GetWithParams(reqUser.Id, paymentHistoryParams);
        return new BaseResponse<PagedList<PaymentHistoryDTO>>(histories);
    }
}