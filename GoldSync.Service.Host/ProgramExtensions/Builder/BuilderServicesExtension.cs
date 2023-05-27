using System.Reflection;
using GoldSync.Service.Contracts.Dtos.Background;
using GoldSync.Service.Contracts.Options;
using GoldSync.Service.Data.Contexts;
using GoldSync.Service.Data.Repositories;
using GoldSync.Service.Host.Flows;
using GoldSync.Service.Host.Services.Background;
using GoldSync.Service.Host.Services.HealthCheck;

namespace GoldSync.Service.Host.ProgramExtensions.Builder;

internal static class BuilderServicesExtension
{
    internal static IServiceCollection ConfigureDbConnection(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<DbSettings>()
            .Bind(configuration.GetSection(nameof(DbSettings)));

        return services;
    }

    internal static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddSingleton<CubDbContext>()
            .AddTransient<CubDbBaseRepository>()
            .AddTransient<AuditRepository>();

        return services;
    }

    internal static IServiceCollection AddFLows(this IServiceCollection services)
    {
        var flowBase = typeof(FlowBase<,>);

        var flows = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(p => p.IsClass && !p.IsAbstract &&
                IsTypeDerivedFromOpenGeneric(p, flowBase))
            .ToList();

        flows.ForEach(f => services.AddTransient(f));

        return services;
    }

    internal static IServiceCollection ConfigurePeriodicHostedService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<PeriodicHostedServiceSettings>()
            .Bind(configuration.GetSection(nameof(PeriodicHostedServiceSettings)));

        return services;
    }

    internal static IServiceCollection AddPeriodicHostedService(this IServiceCollection services)
    {
        services.AddSingleton<BackgroundServiceSettings>();
        services.AddHostedService<PeriodicHostedService>();

        return services;
    }

    internal static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<DbConnectionHealthCheck>(DbSettings.cubDbName);

        return services;
    }

    private static bool IsTypeDerivedFromOpenGeneric(Type type, Type openGeneric)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));
        if (openGeneric is null)
            throw new ArgumentNullException(nameof(openGeneric));
        if (!openGeneric.IsGenericTypeDefinition)
            throw new ArgumentException("Must be a generic type definition.", nameof(openGeneric));

        return (type.IsGenericType && type.GetGenericTypeDefinition() == openGeneric) ||
               (type.BaseType != null && IsTypeDerivedFromOpenGeneric(type.BaseType, openGeneric));
    }
}
