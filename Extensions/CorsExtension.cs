namespace Monolithic.Extensions;

public static class CorsExtension
{
    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        // string[] origins = configuration.GetSection("ELK").Value.Split(",");
        // services.AddCors(c =>
        //     c.AddDefaultPolicy(options =>
        //     {
        //         options.AllowAnyOrigin()
        //                .AllowAnyMethod()
        //                .AllowAnyHeader();
        //     })
        // );
        services.AddCors(c =>
            c.AddDefaultPolicy(options =>
            {
                options.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials()
                       .WithOrigins("http://localhost:3000");
            })
        );
    }
}