namespace Monolithic.Helpers;

public class LoggerManager : ILoggerManager
{
    private readonly ILogger<LoggerManager> _logger;

    public LoggerManager(ILogger<LoggerManager> logger)
    {
        _logger = logger;
    }

    public void LogInformation(string message)
    {
        _logger.LogInformation(message);
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning(message);
    }

    public void LogDebug(string message)
    {
        _logger.LogDebug(message);
    }

    public void LogError(string message)
    {
        _logger.LogError(message);
    }

    public void Log(LogLevel logLevel, string message)
    {
        _logger.Log(logLevel, message);
    }
}