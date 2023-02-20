using GoldSync.Service.Contracts.Defaults;
using GoldSync.Service.Contracts.Dtos.Common;
using GoldSync.Service.Contracts.Enums;
using GoldSync.Service.Contracts.Logger;
using GoldSync.Service.Data.Repositories;
using GoldSync.Service.Host.Services.Background;

namespace GoldSync.Service.Host.Flows;

internal sealed class PhsStopFlow : FlowBase<CancellationToken, Dummy>
{
    private readonly PeriodicHostedService periodicHostedService;
    private readonly AuditRepository auditRepository;

    public PhsStopFlow(ILogger<PhsStopFlow> logger,
        PeriodicHostedService periodicHostedService,
        AuditRepository auditRepository) : base(logger)
    {
        this.periodicHostedService = periodicHostedService;
        this.auditRepository = auditRepository;
    }

    protected override async Task<Dummy> HandleFlow(CancellationToken cancellationToken)
    {
        await this.periodicHostedService.StopAsyncBase(cancellationToken);

        this.periodicHostedService.PeriodicTimer?.Dispose();

        this.logger.LogInformationMessage(
            LogAndExceptionMessagesDefault.PHSStopInfo);

        await this.auditRepository.AddLogProcess(Source.GoldSyncCentral,
            ProcessStatus.Stopped);

        return Dummy.Value;
    }
}
