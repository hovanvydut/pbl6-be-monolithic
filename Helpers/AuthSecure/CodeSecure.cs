using System.Security.Cryptography;

namespace Monolithic.Helpers;

public static class CodeSecure
{
    public static string CreateRandomCode()
    {
        byte[] randomNumber = new byte[32];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}