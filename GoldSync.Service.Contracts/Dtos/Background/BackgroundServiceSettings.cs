namespace GoldSync.Service.Contracts.Dtos.Background;

public sealed class BackgroundServiceSettings
{
    public bool ProcessEnabled { get; set; }

    public TimeSpan PeriodInSeconds { get; set; }
}
