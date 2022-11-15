namespace Monolithic.Models.ReqParams;

public class PostStatisticParams
{
    public string Key { get; set; }

    public string PostIds { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
}