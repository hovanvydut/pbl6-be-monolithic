namespace Monolithic.Common;

public class ELKSettings
{
    public bool Enable { get; set; }

    public string LogstashInputUrl { get; set; }

    public int LogstashInputPort { get; set; }
}