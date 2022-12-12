namespace Monolithic.Common;

public class SentrySettings
{
    public string Dsn { get; set; }
    public string MaxRequestBodySize { get; set; }
    public bool SendDefaultPii { get; set; }
    public string MinimumBreadcrumbLevel { get; set; }
    public string MinimumEventLevel { get; set; }
    public bool AttachStackTrace { get; set; }
    public bool Debug { get; set; }
    public string DiagnosticsLevel { get; set; }
    public double TracesSampleRate { get; set; }
}