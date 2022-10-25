namespace Monolithic.Helpers;

public class JwtSettings
{
    public double Expires { get; set; }

    public string PrivateKey { get; set; }

    public string PublicKey { get; set; }
}