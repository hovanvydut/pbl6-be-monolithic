using Monolithic.Models.Common;

namespace Monolithic.Models.ReqParams;

public class VNPParams : ReqParam
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}