using Serilog.Sinks.Network;
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
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        .MinimumLevel.Override("Monolithic", LogEventLevel.Warning)
                        .MinimumLevel.Override("Sentry", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .Enrich.WithExceptionDetails()
                        .WriteTo.Console()
                        .WriteTo.TCPSink(elkSettings.LogstashInputUrl, elkSettings.LogstashInputPort)
                        // .WriteTo.DurableHttpUsingFileSizeRolledBuffers(requestUri: "http://localhost:8001/pbl6-api")
                        .CreateLogger();

            loggingBuilder.AddSerilog(log);
        }
    }

    public static string GetLogContent(this HttpContext context,
                                       string message,
                                       int statusCode)
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