using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Monolithic.Common;
using Monolithic.Models.Common;
using Monolithic.Models.Context;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;

namespace Monolithic.Services.Implement;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepo;
    private readonly IMapper _mapper;
    private readonly PaymentConfig paymentConfig;
    private readonly DataContext _db;
    private readonly IUserProfileReposiory _userProfileRepo;

    public PaymentService(IPaymentRepository bankCodeRepo, IMapper mapper,
        IOptions<PaymentConfig> paymentConfig, DataContext db,
        IUserProfileReposiory userProfileReposiory)
    {
        _paymentRepo = bankCodeRepo;
        _mapper = mapper;
        this.paymentConfig = paymentConfig.Value;
        _db = db;
        _userProfileRepo = userProfileReposiory;
    }

    public async Task<string> CreatePayement(CreatePaymentDTO createPaymentDTO)
    {
        using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
        {
            try
            {
                // Create DTO
                VNPHistoryDTO vnpHistoryDTO = new VNPHistoryDTO();
                vnpHistoryDTO.vnp_TxnRef = DateTime.Now.Ticks;
                vnpHistoryDTO.vnp_OrderInfo = "#" + vnpHistoryDTO.vnp_TxnRef.ToString() + " | " + createPaymentDTO.OrderDesc;
                vnpHistoryDTO.vnp_Amount = createPaymentDTO.Amount * 100;
                vnpHistoryDTO.vnp_BankCode = createPaymentDTO.BankCode;
                vnpHistoryDTO.vnp_TmnCode = paymentConfig.VNPTmnCode;
                vnpHistoryDTO.vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");

                //Build URL for VNPAY
                VnPayLibrary vnpay = new VnPayLibrary();
                vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnpHistoryDTO.vnp_TmnCode);
                vnpay.AddRequestData("vnp_Amount", vnpHistoryDTO.vnp_Amount.ToString());
                vnpay.AddRequestData("vnp_BankCode", vnpHistoryDTO.vnp_BankCode);
                vnpay.AddRequestData("vnp_CreateDate", vnpHistoryDTO.vnp_CreateDate);
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_Locale", "vn");
                vnpay.AddRequestData("vnp_IpAddr", "8.8.8.8");
                vnpay.AddRequestData("vnp_OrderInfo", vnpHistoryDTO.vnp_OrderInfo);
                vnpay.AddRequestData("vnp_ReturnUrl", paymentConfig.VNPReturnURL);
                vnpay.AddRequestData("vnp_TxnRef", vnpHistoryDTO.vnp_TxnRef.ToString());

                string paymentUrl = vnpay.CreateRequestUrl(paymentConfig.VNPUrl, paymentConfig.VNPHashSecret, vnpHistoryDTO);

                // save payment into db
                await _paymentRepo.CreateVNPHistory(vnpHistoryDTO);

                transaction.Commit();

                return paymentUrl;
            }
            catch (BaseException ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }

    public async Task<List<BankCodeDTO>> GetAllBankCode()
    {
        var bankCodeEntityList = await _paymentRepo.GetAllBankCode();
        return bankCodeEntityList.Select(b => _mapper.Map<BankCodeDTO>(b)).ToList();
    }

    public async Task ReceiveDataFromVNP(VNPayReturnDTO vnpayReturnDTO)
    {
        VnPayLibrary vnpay = new VnPayLibrary();

        // validate hash key
        bool checkSignature = vnpay.ValidateSignature(vnpayReturnDTO.vnp_SecureHash, paymentConfig.VNPHashSecret);

        // TODO: update gold & payment status

        if (checkSignature)
        {
            if (vnpayReturnDTO.vnp_ResponseCode == "00" && vnpayReturnDTO.vnp_TransactionNo == "00")
            {
                Console.WriteLine("giao dich thanh cong");
                VNPHistoryEntity vnpHistoryEntity = await _paymentRepo.GetByTxnRef(vnpayReturnDTO.vnp_TxnRef);
                if (vnpHistoryEntity == null) {
                    Console.WriteLine("Khong ton tai payment #" + vnpayReturnDTO.vnp_TxnRef);
                    return;
                }
                await _userProfileRepo.AddGold(vnpHistoryEntity.UserAccountId, vnpayReturnDTO.vnp_Amount);
            }
            else
            {
                Console.WriteLine("Co loi xay ra trong qua trinh xu ly");
            }
        }
        else
        {
            Console.WriteLine("Co loi xay ra trong qua trinh xu ly");
        }
    }
}