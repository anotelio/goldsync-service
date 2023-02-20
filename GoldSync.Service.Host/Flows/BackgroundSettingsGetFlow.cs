using GoldSync.Service.Contracts.Dtos.Common;
using GoldSync.Service.Host.Services.Background;

namespace GoldSync.Service.Host.Flows;

internal sealed class BackgroundSettingsGetFlow : FlowApiBase<Dummy>
{
    private readonly PeriodicHostedService periodicHostedService;

    public BackgroundSettingsGetFlow(ILogger<BackgroundSettingsGetFlow> logger,
        PeriodicHostedService periodicHostedService) : base(logger)
    {
        this.periodicHostedService = periodicHostedService;
    }

    protected override Task<IResult> HandleFlow(Dummy request)
    {
        return Task.FromResult(
            Results.Ok(this.periodicHostedService.BackgroundServiceSettings));
    }
}
