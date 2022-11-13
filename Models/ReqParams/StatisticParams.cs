namespace Monolithic.Models.ReqParams;

public class StatisticParams
{
    public string Key { get; set; }

    public string UserAccountIds { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}