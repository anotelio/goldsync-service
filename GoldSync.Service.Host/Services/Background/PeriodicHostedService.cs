using GoldSync.Service.Contracts.Dtos.Background;
using GoldSync.Service.Contracts.Dtos.Common;
using GoldSync.Service.Contracts.Options;
using GoldSync.Service.Host.Flows;
using Microsoft.Extensions.Options;

namespace GoldSync.Service.Host.Services.Background;

public sealed class PeriodicHostedService : BackgroundService
{
    public BackgroundServiceSettings BackgroundServiceSettings { get; set; }

    public int ExecuteDelay { get; set; }

    public PeriodicTimer PeriodicTimer { get; set; }

    private readonly IServiceScopeFactory factory;

    public PeriodicHostedService(
        IOptions<PeriodicHostedServiceSettings> periodicHostedServiceOptions,
        IServiceScopeFactory factory)
    {
        BackgroundServiceSettings = new()
        {
            ProcessEnabled =
                (bool)periodicHostedServiceOptions.Value.ProcessEnabled,
            PeriodInSeconds = TimeSpan.FromSeconds(
                (double)periodicHostedServiceOptions.Value.PeriodInSeconds)
        };
        this.factory = factory;
    }

    private async Task RunProcess(Type type, CancellationToken cancellationToken)
    {
        await using AsyncServiceScope asyncScope = factory.CreateAsyncScope();
        var phsProcess = asyncScope.ServiceProvider
            .GetRequiredService(type) as FlowBase<CancellationToken, Dummy>;

        await phsProcess.Run(cancellationToken);
    }

    public Task StartAsyncBase(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    public Task StopAsyncBase(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return this.RunProcess(typeof(PhsStartFlow), cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return this.RunProcess(typeof(PhsStopFlow), cancellationToken);
    }

    public async Task RestartAsync()
    {
        CancellationTokenSource cts = new();

        await this.StopAsync(cts.Token);

        await this.StartAsync(cts.Token);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return this.RunProcess(typeof(PhsExecuteFlow), stoppingToken);
    }
}
