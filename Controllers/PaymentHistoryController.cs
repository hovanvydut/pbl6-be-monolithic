using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using AutoMapper;

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
    [Authorize(Roles = PaymentPermission.ViewAllHistory)]
    public async Task<BaseResponse<PagedList<PaymentHistoryDTO>>> GetWithParams([FromQuery] PaymentHistoryParams paymentHistoryParams)
    {
        var histories = await _paymentHistoryService.GetWithParams(0, paymentHistoryParams);
        return new BaseResponse<PagedList<PaymentHistoryDTO>>(histories);
    }

    [HttpGet("/api/payment-history/personal")]
    [Authorize(Roles = PaymentPermission.ViewAllHistoryPersonal)]
    public async Task<BaseResponse<PagedList<PaymentHistoryDTO>>> GetWithParamsPersonal([FromQuery] PaymentHistoryParams paymentHistoryParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var histories = await _paymentHistoryService.GetWithParams(reqUser.Id, paymentHistoryParams);
        return new BaseResponse<PagedList<PaymentHistoryDTO>>(histories);
    }
}