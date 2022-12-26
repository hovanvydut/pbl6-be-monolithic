using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Monolithic.Common;
using Monolithic.Constants;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Models.ReqParams;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class PaymentController : BaseController
{
    private readonly IPaymentService _paymentService;
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    [Authorize(Roles = VNPPermission.CreatePayment)]
    public async Task<BaseResponse<string>> CreatePayment([FromBody] CreatePaymentDTO createPaymentDTO)
    {
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        string url = await _paymentService.CreatePayment(reqUser.Id, createPaymentDTO);
        return new BaseResponse<string>(url, HttpCode.OK);
    }

    [HttpGet("vnpay-hook-url")]
    public async Task<IActionResult> ReceiveDataFromVNP([FromQuery] VNPayReturnDTO vnpayData)
    {
        await _paymentService.ReceiveDataFromVNP(vnpayData);
        return Ok(vnpayData);
    }

    [HttpGet("bank-code")]
    public async Task<BaseResponse<List<BankCodeDTO>>> GetAllBankCode()
    {
        var result = await _paymentService.GetAllBankCode();
        return new BaseResponse<List<BankCodeDTO>>(result, HttpCode.OK);
    }

    [HttpGet("history")]
    [Authorize(Roles = VNPPermission.ViewAllHistory)]
    public async Task<BaseResponse<PagedList<UserVNPHistoryDTO>>> GetWithParams([FromQuery] VNPParams vnpParams)
    {
        var histories = await _paymentService.GetVNPHistories(0, vnpParams);
        return new BaseResponse<PagedList<UserVNPHistoryDTO>>(histories);
    }

    [HttpGet("history/personal")]
    [Authorize(Roles = VNPPermission.ViewAllHistoryPersonal)]
    public async Task<BaseResponse<PagedList<UserVNPHistoryDTO>>> GetWithParamsPersonal([FromQuery] VNPParams vnpParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var histories = await _paymentService.GetVNPHistories(reqUser.Id, vnpParams);
        return new BaseResponse<PagedList<UserVNPHistoryDTO>>(histories);
    }
}