using GoldSync.Service.Contracts.Defaults;

namespace GoldSync.Service.Host.Flows;

internal abstract class FlowApiBase<T> : FlowBase<T, IResult>
{
    protected FlowApiBase(ILogger<FlowBase<T, IResult>> logger) : base(logger)
    {
    }

    protected override Task<IResult> HandleFlowException(Exception ex, IResult result)
    {
        result = Results.BadRequest(LogAndExceptionMessagesDefault.UnhandledFlowError);

        base.HandleFlowException(ex, result);

        return Task.FromResult(result);
    }
}