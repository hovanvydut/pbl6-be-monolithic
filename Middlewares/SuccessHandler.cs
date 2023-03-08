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

            // If context item is null or false => isError = false
            bool isError = Convert.ToBoolean(context.Items["isError"]);

            if (!isError)
            {
                logger.LogInformation(context.GetLogContent());
            }
        });
    }
}