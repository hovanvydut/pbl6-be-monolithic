using Monolithic.Models.Common;

namespace Monolithic.Models.ReqParams;

public class PostStatisticInDateParams : ReqParam
{
    public string Key { get; set; }

    public DateTime Date { get; set; }

    public bool IncludeDeletedPost { get; set; }
}