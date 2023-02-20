using System.Diagnostics;
using GoldSync.Service.Contracts.Options;
using GoldSync.Service.Data.Repositories;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GoldSync.Service.Host.Services.HealthCheck;

internal class DbConnectionHealthCheck : IHealthCheck
{
    private readonly CubDbBaseRepository cubDbBaseRepository;

    public DbConnectionHealthCheck(CubDbBaseRepository cubDbBaseRepository)
    {
        this.cubDbBaseRepository = cubDbBaseRepository;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        HealthCheckResult hcResult;

        Stopwatch watch = new();
        watch.Start();

        bool isHealthy = this.cubDbBaseRepository.IsDbConnected();

        watch.Stop();

        if (isHealthy)
        {
            if (watch.ElapsedMilliseconds < 1000)
            {
                hcResult = HealthCheckResult.Healthy(
                    string.Concat(DbSettings.cubDbName, ":",
                        Enum.GetName(typeof(HealthStatus), HealthStatus.Healthy)));
            }
            else
            {
                hcResult = HealthCheckResult.Degraded(
                    string.Concat(DbSettings.cubDbName, ":",
                        Enum.GetName(typeof(HealthStatus), HealthStatus.Degraded)));
            }
        }
        else
        {
            hcResult = HealthCheckResult.Unhealthy(
                string.Concat(DbSettings.cubDbName, ":",
                    Enum.GetName(typeof(HealthStatus), HealthStatus.Unhealthy)));
        }

        return Task.FromResult(hcResult);
    }
}
