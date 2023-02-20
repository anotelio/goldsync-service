namespace GoldSync.Service.Contracts.Options;

public sealed class PeriodicHostedServiceSettings
{
    public bool? ProcessEnabled { get; set; }

    public double? PeriodInSeconds { get; set; }

    public static readonly double PeriodInSecondsMin = 1;

    public static readonly double PeriodInSecondsMax = 2592000;

    public void Validate()
    {
        ArgumentNullException.ThrowIfNull(ProcessEnabled);
        ArgumentNullException.ThrowIfNull(PeriodInSeconds);

        if (CheckPeriodInSeconds((double)PeriodInSeconds))
        {
            throw new ArgumentException($"{nameof(PeriodInSeconds)} is not configured correctly.");
        }
    }

    public static bool CheckPeriodInSeconds(double? periodInSeconds)
    {
        return !periodInSeconds.HasValue
            || periodInSeconds < PeriodInSecondsMin
            || periodInSeconds > PeriodInSecondsMax;
    }
}
