using Monolithic.Repositories.Implement;
using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Services.Implement;
using Monolithic.Services.Interface;
using Monolithic.Models.Mapper;
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

    public static void ConfigureDI(this IServiceCollection services)
    {
        services.ConfigureLibraryDI();
        services.ConfigureRepositoryDI();
        services.ConfigureServiceDI();
    }

    private static void ConfigureLibraryDI(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingConfig));
    }

    private static void ConfigureRepositoryDI(this IServiceCollection services)
    {
        services.AddScoped<IExampleRepository, ExampleRepository>();
    }

    private static void ConfigureServiceDI(this IServiceCollection services)
    {
        services.AddScoped<IExampleService, ExampleService>();
    }
}