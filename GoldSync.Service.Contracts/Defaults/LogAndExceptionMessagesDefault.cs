namespace GoldSync.Service.Contracts.Defaults;

public static class LogAndExceptionMessagesDefault
{
    public const string PHSName = "Periodic Hosted Service";

    public const string PHSStartInfo = $"{PHSName} started.";

    public const string PHSStopInfo = $"{PHSName} stopped.";

    public const string PHSOperationCanceledInfo = $"Operation was canceled on {PHSName}.";

    public const string OperationCanceledFlowInfo = "Operation was canceled on the server.";

    public const string ExecutionCountFlowInfo = "Executed {0} - Count: {1} - {2}";

    public const string ExecutionSkippedFlowInfo = "Skipped {0} - {1}";

    public const int PHSExecutionErrorCode = 4101;

    public const string PHSExecutionError = $"Execution error has occured on {PHSName}.";

    public const int UnhandledFlowErrorCode = 4000;

    public const string UnhandledFlowError = "Unhandled internal error has occurred on the server.";

    public const int CriticalErrorCode = 5000;

    public const string CriticalError = "Critical internal error has occurred on the server.";
}
