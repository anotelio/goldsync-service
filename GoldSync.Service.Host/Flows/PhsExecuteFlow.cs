using GoldSync.Service.Contracts.Defaults;
using GoldSync.Service.Contracts.Dtos.Common;
using GoldSync.Service.Contracts.Enums;
using GoldSync.Service.Contracts.Logger;
using GoldSync.Service.Data.Repositories;
using GoldSync.Service.Host.Services.Background;

namespace GoldSync.Service.Host.Flows;

internal sealed class PhsExecuteFlow : FlowBase<CancellationToken, Dummy>
{
    private readonly PeriodicHostedService periodicHostedService;
    private readonly AuditRepository auditRepository;
    private int executionCount;

    public PhsExecuteFlow(ILogger<PhsExecuteFlow> logger,
        PeriodicHostedService periodicHostedService,
        AuditRepository auditRepository) : base(logger)
    {
        this.periodicHostedService = periodicHostedService;
        this.auditRepository = auditRepository;
    }

    protected override async Task<Dummy> HandleFlow(CancellationToken cancellationToken)
    {
        if (this.periodicHostedService.ExecuteDelay != 0)
        {
            await Task.Delay(this.periodicHostedService.ExecuteDelay, cancellationToken);
            this.periodicHostedService.ExecuteDelay = 0;
            this.periodicHostedService.PeriodicTimer = new
                (this.periodicHostedService.BackgroundServiceSettings.PeriodInSeconds);
        }

        do
        {
            if (this.periodicHostedService.BackgroundServiceSettings.ProcessEnabled)
            {
                await this.auditRepository.AddLogProcess(Source.GoldSyncCentral,
                    ProcessStatus.Executing);

                this.executionCount++;

                this.logger.LogInformationMessage(
                    string.Format(LogAndExceptionMessagesDefault.ExecutionCountFlowInfo,
                        LogAndExceptionMessagesDefault.PHSName,
                        this.executionCount,
                        DateTime.Now));

                await this.auditRepository.AddLogProcess(Source.GoldSyncCentral,
                    ProcessStatus.Processed);
            }
            else
            {
                this.logger.LogInformationMessage(
                    string.Format(LogAndExceptionMessagesDefault.ExecutionSkippedFlowInfo,
                        LogAndExceptionMessagesDefault.PHSName,
                        DateTime.Now));

                await this.auditRepository.AddLogProcess(Source.GoldSyncCentral,
                    ProcessStatus.Skipped);
            }
        }
        while (!cancellationToken.IsCancellationRequested &&
            await this.periodicHostedService.PeriodicTimer.WaitForNextTickAsync(cancellationToken));

        return Dummy.Value;
    }

    protected override async Task<Dummy> HandleOperationCanceledFlowException(Dummy result)
    {
        await base.HandleOperationCanceledFlowException(result);

        await auditRepository.AddLogProcess(Source.GoldSyncCentral,
            ProcessStatus.Canceled);

        return Dummy.Value;
    }

    protected override async Task<Dummy> HandleFlowException(Exception ex, Dummy result)
    {
        await base.HandleFlowException(ex, result);

        await auditRepository.AddLogProcess(Source.GoldSyncCentral,
            ProcessStatus.Failed);

        return Dummy.Value;
    }
}
