using Monolithic.Models.Common;

namespace Monolithic.Models.ReqParams;

public class NotificationParams : ReqParam
{
    public bool Today { get; set; }
}