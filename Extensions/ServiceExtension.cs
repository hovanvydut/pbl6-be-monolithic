using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Context;

namespace Monolithic.Extensions;

public static class ServiceExtension
{
    public static void ConfigureDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        string cns = configuration.GetConnectionString("Local");
        services.AddDbContext<DataContext>(options =>
        {
            options.UseMySql(cns, ServerVersion.AutoDetect(cns));
        });
    }
}