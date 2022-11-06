using Monolithic.Constants;

namespace Monolithic.Helpers;

public static class VNPStatus
{
    public static string GetVNPStatus(string vnp_TransactionStatus)
    {
        switch (vnp_TransactionStatus)
        {
            case PaymentConst.VNP_TRANSACTION_STATUS_SUCCESS:
                return "Giao dịch thành công";
            case null:
                return "Không giao dịch";
            default:
                return "Không có thông tin";
        }
    }
}