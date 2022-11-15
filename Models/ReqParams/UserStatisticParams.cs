namespace Monolithic.Models.ReqParams;

public class UserStatisticParams
{
    public string Key { get; set; }

    public string UserIds { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }
}