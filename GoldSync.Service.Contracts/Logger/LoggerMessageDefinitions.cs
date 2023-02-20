using Microsoft.Extensions.Logging;

namespace GoldSync.Service.Contracts.Logger;

public static partial class LoggerMessageDefinitions
{
    [LoggerMessage(EventId = 10,
        Message = "({code}){message}")]
    public static partial void LogDefaultMessage(this ILogger logger, LogLevel level, int code, string message);

    [LoggerMessage(EventId = 20,
        Level = LogLevel.Information,
        Message = "{message}",
        SkipEnabledCheck = true)]
    public static partial void LogInformationMessage(this ILogger logger, string message);

    [LoggerMessage(EventId = 40,
        Level = LogLevel.Error,
        Message = "({code}){message}",
        SkipEnabledCheck = true)]
    public static partial void LogDefaultMessageError(this ILogger logger, int code, string message);

    [LoggerMessage(EventId = 41,
        Level = LogLevel.Error,
        Message = "({code}){message} ::: {aggExMessages}",
        SkipEnabledCheck = true)]
    public static partial void LogExceptionMessageError(this ILogger logger, Exception ex, int code, string message, string aggExMessages);

    [LoggerMessage(EventId = 50,
        Level = LogLevel.Critical,
        Message = "({code}){message} ::: {aggExMessages}",
        SkipEnabledCheck = true)]
    public static partial void LogCriticalError(this ILogger logger, Exception ex, int code, string message, string aggExMessages);
}
