using GoldSync.Service.Host.Middlewares;

namespace GoldSync.Service.Host.ProgramExtensions.Middleware;

internal static class MiddlewareExtension
{
    public static IApplicationBuilder UseCriticalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CriticalExceptionHandlingMiddleware>();
    }
}
