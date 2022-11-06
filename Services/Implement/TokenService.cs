using Monolithic.Repositories.Interface;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Monolithic.Services.Interface;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Monolithic.Models.Entities;
using System.Security.Claims;
using Monolithic.Helpers;

namespace Monolithic.Services.Implement;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IRoleRepository _roleRepo;
    public TokenService(IOptions<JwtSettings> jwtSettings, IRoleRepository roleRepo)
    {
        _jwtSettings = jwtSettings.Value;
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

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(_jwtSettings.Expires),
            SigningCredentials = JwtOptions.GetPrivateKey(_jwtSettings),
            Audience = _jwtSettings.ValidateAudience ? _jwtSettings.ValidAudience : null,
            Issuer = _jwtSettings.ValidateIssuer ? _jwtSettings.ValidIssuer : null,
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}