using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Monolithic.Common;
using Monolithic.Constants;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
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
    public async Task<BaseResponse<string>> CreatePayment([FromBody] CreatePaymentDTO createPaymentDTO)
    {
        string url = await _paymentService.CreatePayement(createPaymentDTO);
        return new BaseResponse<string>(url, HttpCode.OK);
    }

    [HttpGet("vnpay-hook-url")]
    public async Task ReceiveDataFromVNP([FromQuery] VNPayReturnDTO vnpayData)
    {
        await _paymentService.ReceiveDataFromVNP(vnpayData);
    }

    [HttpGet("bank-code")]
    public async Task<BaseResponse<List<BankCodeDTO>>> GetAllBankCode()
    {
        var result = await _paymentService.GetAllBankCode();
        return new BaseResponse<List<BankCodeDTO>>(result, HttpCode.OK);
    }
}