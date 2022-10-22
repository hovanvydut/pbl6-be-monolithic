using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Monolithic.Common;
using Monolithic.Constants;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class PaymentController : BaseController
{
    private readonly PaymentConfig paymentConfig;
    private readonly IPaymentService _paymentService;
    public PaymentController(IOptions<PaymentConfig> paymentConfig, IPaymentService paymentService)
    {
        this.paymentConfig = paymentConfig.Value;
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDTO createPaymentDTO)
    {
        PaymentInfoDTO paymentInfo = new PaymentInfoDTO();

        Console.WriteLine(paymentConfig);

        //Save order to db
        paymentInfo.OrderId = DateTime.Now.Ticks;
        paymentInfo.Amount = createPaymentDTO.Amount;
        paymentInfo.Status = "0";
        paymentInfo.OrderDesc = createPaymentDTO.OrderDesc;
        paymentInfo.CreatedDate = DateTime.Now;
        paymentInfo.BankCode = createPaymentDTO.BankCode;

        //Build URL for VNPAY
        VnPayLibrary vnpay = new VnPayLibrary();

        vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
        vnpay.AddRequestData("vnp_Command", "pay");
        vnpay.AddRequestData("vnp_TmnCode", paymentConfig.VNPTmnCode);
        vnpay.AddRequestData("vnp_Amount", (paymentInfo.Amount * 100).ToString());
        vnpay.AddRequestData("vnp_BankCode", paymentInfo.BankCode);
        vnpay.AddRequestData("vnp_CreateDate", paymentInfo.CreatedDate.ToString("yyyyMMddHHmmss"));
        vnpay.AddRequestData("vnp_CurrCode", "VND");

        vnpay.AddRequestData("vnp_Locale", "vn");
        vnpay.AddRequestData("vnp_IpAddr", "8.8.8.8");
        vnpay.AddRequestData("vnp_OrderInfo", "#" + paymentInfo.OrderId + " | " + paymentInfo.OrderDesc);
        vnpay.AddRequestData("vnp_ReturnUrl", paymentConfig.VNPReturnURL);
        vnpay.AddRequestData("vnp_TxnRef", paymentInfo.OrderId.ToString()); 

        string paymentUrl = vnpay.CreateRequestUrl(paymentConfig.VNPUrl, paymentConfig.VNPHashSecret);
        Console.WriteLine("VNPAY URL: {0}", paymentUrl);
        return Ok(paymentUrl);
    }

    [HttpGet]
    public IActionResult ReceiveDataFromVNP([FromQuery] VNPayReturnDTO vnpayData)
    {
        return Ok(vnpayData);
    }

    [HttpGet("bank-code")]
    public async Task<BaseResponse<List<BankCodeDTO>>> GetAllBankCode()
    {
        var result = await _paymentService.GetAllBankCode();
        return new BaseResponse<List<BankCodeDTO>>(result, HttpCode.OK);
    }
}