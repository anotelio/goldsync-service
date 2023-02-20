namespace GoldSync.Service.Host.ProgramExtensions;

internal static class ProgramExtension
{
    internal static bool IsLocal(this IHostEnvironment env)
    {
        return env.IsEnvironment("Local");
    }
}
