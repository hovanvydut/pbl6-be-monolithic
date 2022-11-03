using Monolithic.Models.Common;

namespace Monolithic.Models.ReqParams;
public class BookingParams : ReqParam
{
    public int month { get; set; }
    public int year { get; set; }
}