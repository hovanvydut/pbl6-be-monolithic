using Monolithic.Models.Common;

namespace Monolithic.Models.ReqParams;

public class PostTableListParams : ReqParam
{
    public bool Priority { get; set; }

    public bool Deleted { get; set; }
}