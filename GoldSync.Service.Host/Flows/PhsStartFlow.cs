using GoldSync.Service.Contracts.Defaults;
using GoldSync.Service.Contracts.Dtos.Common;
using GoldSync.Service.Contracts.Enums;
using GoldSync.Service.Contracts.Logger;
using GoldSync.Service.Data.Repositories;
using GoldSync.Service.Host.Services.Background;

namespace GoldSync.Service.Host.Flows;

internal sealed class PhsStartFlow : FlowBase<CancellationToken, Dummy>
{
    private readonly PeriodicHostedService periodicHostedService;
    private readonly AuditRepository auditRepository;

    public PhsStartFlow(ILogger<PhsStartFlow> logger,
        PeriodicHostedService periodicHostedService,
        AuditRepository auditRepository) : base(logger)
    {
        this.periodicHostedService = periodicHostedService;
        this.auditRepository = auditRepository;
    }

    protected override async Task<Dummy> HandleFlow(CancellationToken cancellationToken)
    {
        this.periodicHostedService.ExecuteDelay = 2000;

        await this.periodicHostedService.StartAsyncBase(cancellationToken);

        this.logger.LogInformationMessage(
            LogAndExceptionMessagesDefault.PHSStartInfo);

        await this.auditRepository.AddLogProcess(Source.GoldSyncCentral,
            ProcessStatus.Started);

        return Dummy.Value;
    }
}
