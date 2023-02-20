using GoldSync.Service.Contracts.Defaults;
using GoldSync.Service.Contracts.Exceptions;
using GoldSync.Service.Contracts.Logger;

namespace GoldSync.Service.Host.Middlewares;

public sealed class CriticalExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<CriticalExceptionHandlingMiddleware> logger;

    public CriticalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<CriticalExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (Exception ex)
        {
            await this.HandleCriticalExceptionAsync(ex);
        }
    }

    private Task HandleCriticalExceptionAsync(Exception ex)
    {
        this.logger.LogCriticalError(ex,
            LogAndExceptionMessagesDefault.CriticalErrorCode,
            LogAndExceptionMessagesDefault.CriticalError,
            ex.AggregateExceptionMessages());

        return Task.CompletedTask;
    }
}
