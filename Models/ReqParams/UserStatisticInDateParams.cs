using Monolithic.Models.Common;

namespace Monolithic.Models.ReqParams;

public class UserStatisticInDateParams : ReqParam
{
    public string Key { get; set; }

    public DateTime Date { get; set; }
}