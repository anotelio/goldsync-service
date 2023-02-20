using GoldSync.Service.Contracts.Dtos.Common;
using GoldSync.Service.Contracts.Dtos.Requests;
using GoldSync.Service.Contracts.Options;
using GoldSync.Service.Host.Flows;
using Microsoft.Extensions.Options;

namespace GoldSync.Service.Host.ProgramExtensions.Application;

internal static class WebApplicationExtension
{
    internal static IApplicationBuilder OptionsValidate(this IApplicationBuilder app)
    {
        var dbOptions = app.ApplicationServices.GetRequiredService<IOptions<DbSettings>>();
        var periodicHostedServiceOptions =
            app.ApplicationServices.GetRequiredService<IOptions<PeriodicHostedServiceSettings>>();

        dbOptions.Value.Validate();
        periodicHostedServiceOptions.Value.Validate();

        return app;
    }

    internal static IEndpointRouteBuilder MapGetBackgroundSettings(
        this IEndpointRouteBuilder endpointBuilder,
        string pattern)
    {
        Func<BackgroundSettingsGetFlow, Task<IResult>> handler =
            async (BackgroundSettingsGetFlow flow) =>
                await flow.Run(Dummy.Value);

        endpointBuilder
            .MapGet(pattern, handler)
            .WithName("GetBackgroundSettingsOnPeriodicHostedService");

        return endpointBuilder;
    }

    internal static IEndpointRouteBuilder MapPostBackgroundSettings(
        this IEndpointRouteBuilder endpointBuilder,
        string pattern)
    {
        Func<BackgroundSettingsRequest, BackgroundSettingsSetupFlow, Task<IResult>> handler =
            async (BackgroundSettingsRequest request, BackgroundSettingsSetupFlow flow) =>
                await flow.Run(request);

        endpointBuilder
            .MapPost(pattern, handler)
            .WithName("PostBackgroundSettingsOnPeriodicHostedService");

        return endpointBuilder;
    }

    internal static IEndpointRouteBuilder MapGetServiceRestart(
        this IEndpointRouteBuilder endpointBuilder,
        string pattern)
    {
        Func<PhsRestartApiFlow, Task<IResult>> handler =
            async (PhsRestartApiFlow flow) =>
                await flow.Run(Dummy.Value);

        endpointBuilder
            .MapGet(pattern, handler)
            .WithName("GetRestartOnPeriodicHostedService");

        return endpointBuilder;
    }
}
