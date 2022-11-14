using Monolithic.Models.Common;

namespace Monolithic.Models.ReqParams;

public class PostSearchFilterParams : ReqParam
{
    public string Properties { get; set; }

    public float MinPrice { get; set; }
    public float MaxPrice { get; set; }

    public float MinArea { get; set; }
    public float MaxArea { get; set; }

    public int AddressWardId { get; set; }

    public int CategoryId { get; set; }
}