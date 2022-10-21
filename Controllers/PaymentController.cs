using Microsoft.AspNetCore.Mvc;
using Monolithic.Common;
using Monolithic.Models.DTO;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class PaymentController : BaseController
{
    public PaymentController()
    {

    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] PaymentInfoDTO paymentInfo)
    {
        string ngrok = "https://408c-2405-4802-606c-6be0-5cb-fef9-7086-f7d7.ap.ngrok.io";
        string vnp_Returnurl = ngrok + "/api/payment";
        string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
        string vnp_TmnCode = "WVAVN7JM";
        string vnp_HashSecret = "YMUNAHNFCJKOELDLPWRQLLNUOMQZEQXR";

        //Get payment input
        PaymentInfoDTO order = new PaymentInfoDTO();
        //Save order to db
        order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
        order.Amount = paymentInfo.Amount; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
        order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
        order.OrderDesc = "AAAAAAA";
        order.CreatedDate = DateTime.Now;
        string locale = "vn";
        //Build URL for VNPAY
        VnPayLibrary vnpay = new VnPayLibrary();

        vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
        vnpay.AddRequestData("vnp_Command", "pay");
        vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
        vnpay.AddRequestData("vnp_BankCode", "NCB");
        vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
        vnpay.AddRequestData("vnp_CurrCode", "VND");
        // vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
        // if (!string.IsNullOrEmpty(locale))
        // {
        //     vnpay.AddRequestData("vnp_Locale", locale);
        // }
        // else
        // {
        //     vnpay.AddRequestData("vnp_Locale", "vn");
        // }
        vnpay.AddRequestData("vnp_Locale", "vn");
        vnpay.AddRequestData("vnp_IpAddr", "8.8.8.8");
        vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang xxxx:" + order.OrderId);
        vnpay.AddRequestData("vnp_OrderType", "topup"); //default value: other
        vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
        vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

        //Add Params of 2.1.0 Version
        // vnpay.AddRequestData("vnp_ExpireDate", txtExpire.Text);
        // //Billing
        // vnpay.AddRequestData("vnp_Bill_Mobile", txt_billing_mobile.Text.Trim());
        // vnpay.AddRequestData("vnp_Bill_Email", txt_billing_email.Text.Trim());
        // var fullName = txt_billing_fullname.Text.Trim();
        // if (!String.IsNullOrEmpty(fullName))
        // {
        //     var indexof = fullName.IndexOf(' ');
        //     vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
        //     vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(indexof + 1, fullName.Length - indexof - 1));
        // }
        // vnpay.AddRequestData("vnp_Bill_Address", txt_inv_addr1.Text.Trim());
        // vnpay.AddRequestData("vnp_Bill_City", txt_bill_city.Text.Trim());
        // vnpay.AddRequestData("vnp_Bill_Country", txt_bill_country.Text.Trim());
        // vnpay.AddRequestData("vnp_Bill_State", "");
        // // Invoice
        // vnpay.AddRequestData("vnp_Inv_Phone", txt_inv_mobile.Text.Trim());
        // vnpay.AddRequestData("vnp_Inv_Email", txt_inv_email.Text.Trim());
        // vnpay.AddRequestData("vnp_Inv_Customer", txt_inv_customer.Text.Trim());
        // vnpay.AddRequestData("vnp_Inv_Address", txt_inv_addr1.Text.Trim());
        // vnpay.AddRequestData("vnp_Inv_Company", txt_inv_company.Text);
        // vnpay.AddRequestData("vnp_Inv_Taxcode", txt_inv_taxcode.Text);
        // vnpay.AddRequestData("vnp_Inv_Type", cbo_inv_type.SelectedItem.Value);

        string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        Console.WriteLine("VNPAY URL: {0}", paymentUrl);
        return Ok(paymentUrl);
    }

    [HttpGet]
    public IActionResult ReceiveDataFromVNP([FromQuery] VNPayReturnDTO vnpayData)
    {
        return Ok(vnpayData);
    }
}