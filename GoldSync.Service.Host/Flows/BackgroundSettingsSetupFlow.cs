using System.Transactions;
using GoldSync.Service.Contracts.Defaults;
using GoldSync.Service.Contracts.Dtos.Requests;
using GoldSync.Service.Contracts.Options;
using GoldSync.Service.Data.Repositories;
using GoldSync.Service.Host.Services.Background;
using GoldSync.Service.Contracts.Enums;

namespace GoldSync.Service.Host.Flows;

internal sealed class BackgroundSettingsSetupFlow : FlowApiBase<BackgroundSettingsRequest>
{
    private readonly PeriodicHostedService periodicHostedService;
    private readonly AuditRepository auditRepository;

    public BackgroundSettingsSetupFlow(ILogger<BackgroundSettingsSetupFlow> logger,
        PeriodicHostedService periodicHostedService,
        AuditRepository auditRepository) : base(logger)
    {
        this.periodicHostedService = periodicHostedService;
        this.auditRepository = auditRepository;
    }

    protected override async Task<IResult> HandleFlow(BackgroundSettingsRequest request)
    {
        if (IsValidRequest(request))
        {
            return Results.BadRequest(GetBadMessage());
        }

        (bool enabledEqual, bool periodEqual, bool enabled) changes =
            CheckChanges(request);

        if (changes.enabledEqual && changes.periodEqual)
        {
            return Results.BadRequest(GetNoChangesBadMessage());
        }

        string okMessage = SetProcessEnabled(changes);

        okMessage += await SetPeriodInSeconds(request, changes.periodEqual, okMessage);

        await AddSettingsToDb(request);

        return Results.Ok(okMessage);
    }

    private static bool IsValidRequest(BackgroundSettingsRequest request)
    {
        return PeriodicHostedServiceSettings.CheckPeriodInSeconds(request.PeriodInSeconds)
            || !request.ProcessEnabled.HasValue;
    }

    private string GetBadMessage()
    {
        return string.Format(
            HostApiMessagesDefault.BackgroundSettingsBadMessage,
            nameof(this.periodicHostedService.BackgroundServiceSettings),
            LogAndExceptionMessagesDefault.PHSName,
            nameof(this.periodicHostedService.BackgroundServiceSettings.PeriodInSeconds),
            PeriodicHostedServiceSettings.PeriodInSecondsMin,
            PeriodicHostedServiceSettings.PeriodInSecondsMax,
            nameof(this.periodicHostedService.BackgroundServiceSettings.ProcessEnabled));
    }

    private (bool, bool, bool) CheckChanges(BackgroundSettingsRequest request)
    {
        bool enabled = Convert.ToBoolean(request.ProcessEnabled);
        bool periodEqual = request.PeriodInSeconds ==
            this.periodicHostedService.BackgroundServiceSettings.PeriodInSeconds.TotalSeconds;
        bool enabledEqual = enabled ==
            this.periodicHostedService.BackgroundServiceSettings.ProcessEnabled;

        return (enabledEqual, periodEqual, enabled);
    }

    private static string GetNoChangesBadMessage()
    {
        return string.Format(
            HostApiMessagesDefault.BackgroundSettingsNoChangesBadMessage,
            LogAndExceptionMessagesDefault.PHSName);
    }

    private string SetProcessEnabled((bool enabledEqual, bool periodEqual, bool enabled) changes)
    {
        string okMessage = string.Empty;

        if (!changes.enabledEqual)
        {
            this.periodicHostedService.BackgroundServiceSettings.ProcessEnabled = changes.enabled;

            okMessage += string.Format(
                HostApiMessagesDefault.ProcessEnabledOkMessage,
                nameof(this.periodicHostedService.BackgroundServiceSettings.ProcessEnabled),
                LogAndExceptionMessagesDefault.PHSName,
                this.periodicHostedService.BackgroundServiceSettings.ProcessEnabled);

            if (!changes.periodEqual)
            {
                okMessage += Environment.NewLine;
            }
        }

        return okMessage;
    }

    private async Task<string> SetPeriodInSeconds(BackgroundSettingsRequest request,
        bool periodEqual, string okMessage)
    {
        if (!periodEqual)
        {
            this.periodicHostedService.BackgroundServiceSettings.PeriodInSeconds =
                TimeSpan.FromSeconds((double)request.PeriodInSeconds);

            okMessage += string.Format(
                HostApiMessagesDefault.PeriodInSecondsOkMessage,
                nameof(this.periodicHostedService.BackgroundServiceSettings.PeriodInSeconds),
                LogAndExceptionMessagesDefault.PHSName,
                this.periodicHostedService.BackgroundServiceSettings.PeriodInSeconds.TotalSeconds);

            await this.periodicHostedService.RestartAsync();
        }

        return okMessage;
    }

    private async Task AddSettingsToDb(BackgroundSettingsRequest request)
    {
        using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);

        int processId = await this.auditRepository
            .AddLogProcess(Source.GoldSyncCentral, ProcessStatus.Setup);

        await this.auditRepository
            .AddLogProcessWithData(processId, request.Serialized());

        scope.Complete();
    }
}