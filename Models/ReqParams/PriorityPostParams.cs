using Monolithic.Models.Common;

namespace Monolithic.Models.ReqParams;

public class PriorityPostParams : ReqParam
{
    public int AddressWardId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }
}