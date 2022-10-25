using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Monolithic.Helpers;

public static class JwtOptions
{
    public static TokenValidationParameters GetTokenValidateParams(byte[] publicKey)
    {
        using RSA rsa = RSA.Create();
        rsa.ImportRSAPublicKey(publicKey, out var _);
        return new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            CryptoProviderFactory = new CryptoProviderFactory()
            {
                CacheSignatureProviders = false
            },
        };
    }

    public static byte[] ToByteArray(this string key)
    {
        return Convert.FromBase64String(key);
    }
}