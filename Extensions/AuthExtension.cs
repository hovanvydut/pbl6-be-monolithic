using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Monolithic.Extensions;

public static class AuthExtension
{
    public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureJWT(configuration);
        services.ConfigureSwaggerForAuth();
    }

    private static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenKey = Encoding.UTF8.GetBytes(configuration["JwtSettings:secretKey"]);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(tokenKey)
                };
            });
    }

    private static void ConfigureSwaggerForAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"Enter 'Bearer ' + your Token   
                                    Example: Bearer 123456789",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
            });
        });
    }
}