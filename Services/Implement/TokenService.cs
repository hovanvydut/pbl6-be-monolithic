using Monolithic.Repositories.Interface;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Monolithic.Services.Interface;
using Monolithic.Models.Entities;
using System.Security.Claims;
using Monolithic.Helpers;
using System.Text;

namespace Monolithic.Services.Implement;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IRoleRepository _roleRepo;
    public TokenService(IConfiguration configuration, IRoleRepository roleRepo)
    {
        _configuration = configuration;
        _roleRepo = roleRepo;
    }

    public async Task<string> CreateToken(UserAccountEntity user)
    {
        var listPermission = await _roleRepo.GetPermissionByRoleId(user.RoleId);

        var claims = new List<Claim>();
        claims.Add(new Claim(CustomClaimTypes.UserId, user.Id.ToString()));
        claims.Add(new Claim(CustomClaimTypes.Email, user.Email));
        foreach (var permission in listPermission)
        {
            claims.Add(new Claim(CustomClaimTypes.Permission, permission.Key));
        }

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