using System.Security.Cryptography;
using System.Text;

namespace Monolithic.Helpers.Password;

public static class PasswordSecure
{
    public static PasswordHash GetPasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        var passwordByte = Encoding.UTF8.GetBytes(password);
        return new PasswordHash()
        {
            PasswordHashed = hmac.ComputeHash(passwordByte),
            PasswordSalt = hmac.Key
        };
    }

    public static bool IsValidPasswod(string password, PasswordHash passwordHash)
    {
        using var hmac = new HMACSHA512(passwordHash.PasswordSalt);
        var passwordByte = Encoding.UTF8.GetBytes(password);
        var computedHash = hmac.ComputeHash(passwordByte);
        return computedHash.SequenceEqual(passwordHash.PasswordHashed);
    }
}