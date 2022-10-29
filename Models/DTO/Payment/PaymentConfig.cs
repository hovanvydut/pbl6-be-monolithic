namespace Monolithic.Models.DTO;

public class PaymentConfig
{
    public string VNPReturnURL { get; set; }
    public string VNPUrl { get; set; }
    public string VNPTmnCode { get; set; }
    public string VNPHashSecret { get; set; }

    public override string ToString()
    {
        return String.Format("PaymentInfo (VNPReturnURL={0}, VNPUrl={1}, VNPTmnCode={2}, VNPHashSecret={3}", VNPReturnURL, VNPUrl, VNPTmnCode, VNPHashSecret);
    }
}