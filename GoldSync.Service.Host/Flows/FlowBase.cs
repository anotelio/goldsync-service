using GoldSync.Service.Contracts.Defaults;
using GoldSync.Service.Contracts.Exceptions;
using GoldSync.Service.Contracts.Logger;

namespace GoldSync.Service.Host.Flows;

internal abstract class FlowBase<T, R>
{
    protected readonly ILogger<FlowBase<T, R>> logger;

    protected FlowBase(ILogger<FlowBase<T, R>> logger)
    {
        this.logger = logger;
    }

    public async Task<R> Run(T request)
    {
        R result = default;

        try
        {
            result = await HandleFlow(request);
        }
        catch (OperationCanceledException)
        {
            result = await HandleOperationCanceledFlowException(result);
        }
        catch (Exception ex)
        {
            result = await HandleFlowException(ex, result);
        }

        return result;
    }

    protected abstract Task<R> HandleFlow(T request);

    protected virtual Task<R> HandleOperationCanceledFlowException(R result)
    {
        this.logger.LogInformationMessage(
            LogAndExceptionMessagesDefault.OperationCanceledFlowInfo);

        return Task.FromResult(result);
    }

    protected virtual Task<R> HandleFlowException(Exception ex, R result)
    {
        string errorMessages = ex.AggregateExceptionMessages();
        this.logger.LogExceptionMessageError(
            ex,
            LogAndExceptionMessagesDefault.UnhandledFlowErrorCode,
            LogAndExceptionMessagesDefault.UnhandledFlowError,
            errorMessages);

        return Task.FromResult(result);
    }
}