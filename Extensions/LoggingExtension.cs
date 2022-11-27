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
                        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                        .Enrich.FromLogContext()
                        .Enrich.WithExceptionDetails()
                        .WriteTo.Console()
                        .WriteTo.TCPSink(elkSettings.LogstashInputUrl, elkSettings.LogstashInputPort)
                        // .WriteTo.DurableHttpUsingFileSizeRolledBuffers(requestUri: "http://localhost:8001/pbl6-api")
                        .CreateLogger();

            loggingBuilder.AddSerilog(log);
        }
    }
}