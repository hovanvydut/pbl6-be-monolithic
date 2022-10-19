using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Monolithic.Models.Common;
using Monolithic.Constants;
using Monolithic.Helpers;
using System.Text;

namespace Monolithic.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null) getDataFromTokenPayload(context, token);
        await _next(context);
    }

    private void getDataFromTokenPayload(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JwtSettings:secretKey"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            // Get data from payload
            context.Items["reqUser"] = new ReqUser()
            {
                Id = Convert.ToInt32(jwtToken.Claims.First(x => x.Type == CustomClaimTypes.UserId).Value),
                Email = jwtToken.Claims.First(x => x.Type == CustomClaimTypes.Email).Value,
            };
        }
        catch
        {
            throw new BaseException(HttpCode.UNAUTHORIZED, "Invalid token");
        }
    }
}