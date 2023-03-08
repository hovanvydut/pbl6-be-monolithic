using Serilog.Sinks.Network;
using Monolithic.Constants;
using Serilog.Exceptions;
using Monolithic.Common;
using Serilog.Events;
using Serilog;

namespace Monolithic.Extensions;

public static class LoggingExtension
{
    public static void ConfigureSerilog(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        var elkSettings = configuration.GetSection("ELK").Get<ELKSettings>();

        if (elkSettings.Enable)
        {
            var log = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
                        .CreateLogger();

            loggingBuilder.AddSerilog(log);
        }
    }

    public static string GetLogContent(this HttpContext context,
                                       string message = "",
                                       int statusCode = HttpCode.OK)
    {
        LogContent content = new LogContent
        {
            Path = context.Request.Path,
            Method = context.Request.Method,
            Params = "",
            Message = message,
            StatusCode = statusCode,
        };
        QueryString queryString = context.Request.QueryString;
        if (queryString.HasValue)
        {
            content.Params = queryString.Value;
        }
        return content.ToString();
    }
}