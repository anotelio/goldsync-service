using GoldSync.Service.Contracts.Defaults;
using GoldSync.Service.Contracts.Dtos.Common;
using GoldSync.Service.Host.Services.Background;

namespace GoldSync.Service.Host.Flows;

internal sealed class PhsRestartApiFlow : FlowApiBase<Dummy>
{
    private readonly PeriodicHostedService periodicHostedService;

    public PhsRestartApiFlow(ILogger<PhsRestartApiFlow> logger,
        PeriodicHostedService periodicHostedService) : base(logger)
    {
        this.periodicHostedService = periodicHostedService;
    }

    protected override async Task<IResult> HandleFlow(Dummy request)
    {
        await this.periodicHostedService.RestartAsync();

        return Results.Ok(string.Format(
            HostApiMessagesDefault.RestartOkMessage,
            LogAndExceptionMessagesDefault.PHSName));
    }
}
