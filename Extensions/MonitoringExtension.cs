using Monolithic.Common;
using Serilog;

namespace Monolithic.Extensions;

public static class MonitoringExtension
{
    public static void ConfigureSentry(this IWebHostBuilder webHostBuilder, IConfiguration configuration)
    {
        var sentrySettings = configuration.GetSection("Sentry").Get<SentrySettings>();

        webHostBuilder.UseSentry(options =>
        {
            options.Dsn = sentrySettings.Dsn;
            // options.MaxRequestBodySize = sentrySettings.MaxRequestBodySize;
            options.SendDefaultPii = sentrySettings.SendDefaultPii;
            // options.MinimumBreadcrumbLevel = sentrySettings.MinimumBreadcrumbLevel;
            // options.MinimumEventLevel = sentrySettings.MinimumEventLevel;
            options.AttachStacktrace = sentrySettings.AttachStackTrace;
            options.Debug = sentrySettings.Debug;
            // options.DiagnosticLevel = sentrySettings.DiagnosticsLevel;
            options.TracesSampleRate = sentrySettings.TracesSampleRate;
        });
    }
}