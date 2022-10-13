using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Monolithic.Services.Interface;
using Monolithic.Models.Entities;
using System.Security.Claims;
using System.Text;

namespace Monolithic.Services.Implement;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(UserAccountEntity user)
    {
        var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

        var tokenKey = Encoding.UTF8.GetBytes(_configuration["JwtSettings:secretKey"]);
        var tokenLife = Convert.ToDouble(_configuration["JwtSettings:expires"]);

        var symmetricKey = new SymmetricSecurityKey(tokenKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(tokenLife),
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}