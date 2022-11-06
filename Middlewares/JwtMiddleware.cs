using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Monolithic.Models.Common;
using Monolithic.Helpers;
using System.Text;

namespace Monolithic.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtSettings _jwtSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings)
    {
        _next = next;
        _jwtSettings = jwtSettings.Value;
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
            var publicKey = JwtOptions.GetPublicKey(_jwtSettings);
            var tokenValidationParameters = JwtOptions.GetTokenParams(_jwtSettings, publicKey);
            tokenHandler.ValidateToken(token,
                tokenValidationParameters, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            // Get data from payload
            context.Items["reqUser"] = new ReqUser()
            {
                Id = Convert.ToInt32(jwtToken.Claims.First(x => x.Type == CustomClaimTypes.UserId).Value),
                Email = jwtToken.Claims.First(x => x.Type == CustomClaimTypes.Email).Value,
            };
        }
        catch { }
    }
}