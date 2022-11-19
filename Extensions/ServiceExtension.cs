using Monolithic.Repositories.Implement;
using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Services.Implement;
using Monolithic.Services.Interface;
using Monolithic.Models.Context;
using Monolithic.Models.Mapper;
using Microsoft.OpenApi.Models;
using Monolithic.Helpers;
using Monolithic.Common;
using Monolithic.Models.DTO;

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
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<ClientAppSettings>(configuration.GetSection("ClientAppSettings"));

        string currentDatabaseConfig = configuration.GetSection("CurrentDatabaseConfig").Value;
        Console.WriteLine(configuration.GetSection("PaymentConfig").GetSection(currentDatabaseConfig).Value);
        services.Configure<PaymentConfig>(configuration.GetSection("PaymentConfig").GetSection(currentDatabaseConfig));
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
        services.AddScoped<IUserProfileReposiory, UserProfileReposiory>();
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IMediaRepository, MediaRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IBookmarkRepository, BookmarkRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IConfigSettingRepository, ConfigSettingRepository>();
        services.AddScoped<IPaymentHistoryRepository, PaymentHistoryRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IPriorityPostRepository, PriorityPostRepository>();
        services.AddScoped<IStatisticRepository, StatisticRepository>();
    }

    private static void ConfigureServiceDI(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPropertyService, PropertyService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IBookmarkService, BookmarkService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IConfigSettingService, ConfigSettingService>();
        services.AddScoped<IPaymentHistoryService, PaymentHistoryService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<IPriorityPostService, PriorityPostService>();
        services.AddScoped<IStatisticService, StatisticService>();
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