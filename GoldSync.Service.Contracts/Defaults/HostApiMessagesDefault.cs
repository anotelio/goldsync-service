namespace GoldSync.Service.Contracts.Defaults;

public static class HostApiMessagesDefault
{
    /// <summary>
    /// Setting enabled process of periodic timer Ok message format:
    /// 0 - ProcessEnabled name
    /// 1 - ServiceName
    /// 2 - ProcessEnabled
    /// </summary>
    public const string ProcessEnabledOkMessage = "{0} on {1} was set to {2}.";

    /// <summary>
    /// Setting Background Settings of periodic timer Bad message format:
    /// 0 - BackgroundSettings
    /// 1 - ServiceName
    /// 2 - PeriodInSeconds
    /// 3 - PeriodInSecondsMin
    /// 4 - PeriodInSecondsMax
    /// 5 - ProcessEnabled
    /// </summary>
    public const string BackgroundSettingsBadMessage = "{0} on {1} was not set. " +
        "A new value for {2} must be between {3} and {4}. " +
        "A new value for {5} must be non-nullable integer.";

    /// <summary>
    /// Background Settings request has no changes Bad message format:
    /// 0 - ServiceName
    /// </summary>
    public const string BackgroundSettingsNoChangesBadMessage = "Settings in the request are already applied for {0}";

    /// <summary>
    /// Setting period in seconds of periodic timer Ok message format:
    /// 0 - PeriodInSeconds
    /// 1 - ServiceName
    /// 2 - PeriodInSeconds
    /// </summary>
    public const string PeriodInSecondsOkMessage = "{0} on {1} was set to {2}.";

    /// <summary>
    /// Service restarted Ok message format:
    /// 0 - ServiceName
    /// </summary>
    public const string RestartOkMessage = "{0} restarted.";
}
