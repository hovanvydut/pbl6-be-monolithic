using Monolithic.Repositories.Implement;
using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Services.Implement;
using Monolithic.Services.Interface;
using Monolithic.Models.Context;
using Monolithic.Common;
using Monolithic.Models.Mapper;
using Monolithic.Helpers;
using Microsoft.OpenApi.Models;

namespace Monolithic.Extensions;

public static class ServiceExtension
{
    public static void ConfigureDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        string currentDatabaseConfig = configuration.GetSection("CurrentDatabaseConfig").Value;
        string cns = configuration.GetConnectionString(currentDatabaseConfig);
        services.AddDbContext<DataContext>(options =>
        {
            options.UseMySql(cns, ServerVersion.AutoDetect(cns));
        });
    }

    public static void ConfigureModelSetting(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
    }

    public static void ConfigureDI(this IServiceCollection services, IConfigUtil configUtil)
    {
        services.ConfigureLibraryDI();
        services.ConfigureRepositoryDI();
        services.ConfigureServiceDI();
        services.ConfigCommonServiceDI();
        services.ConfigureHelperDI();
        services.ConfigSwagger(configUtil);
    }

    private static void ConfigureLibraryDI(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingConfig));
    }

    private static void ConfigureRepositoryDI(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserAccountReposiory, UserAccountReposiory>();
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
    }

    private static void ConfigureServiceDI(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserAccountService, UserAccountService>();
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IAddressService, AddressService>();
    }

    private static void ConfigCommonServiceDI(this IServiceCollection services)
    {
        services.AddScoped<IConfigUtil, ConfigUtil>();
        services.AddScoped<IAuthService, AuthService>();
    }

    private static void ConfigureHelperDI(this IServiceCollection services)
    {
        services.AddScoped<ISendMailHelper, SendMailHelper>();
    }

    private static void ConfigSwagger(this IServiceCollection services, IConfigUtil configUtil)
    {
        // Register the Swagger generator, defining 1 or more Swagger documents
        services.AddSwaggerGen(c =>
        {
            string version = "v" + configUtil.getAPIVersion();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API PBL6", Version = version });
        });
    }
}