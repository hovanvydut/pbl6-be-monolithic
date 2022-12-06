using Monolithic.Extensions;
using Monolithic.Helpers;

namespace Monolithic.Middlewares;

public static class SuccessHandler
{
    public static void ConfigureSuccessHandler(this IApplicationBuilder app, ILoggerManager logger)
    {
        app.Use(async (context, next) =>
        {
            await next();
            logger.LogInformation(context.GetLogContent());
        });
    }
}