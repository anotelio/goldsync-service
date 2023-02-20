namespace GoldSync.Service.Contracts.Defaults;

public static class SqlExecutionsDefault
{
    public const string AddLogProcessSp = "[goldsync].[AddLogProcess]";
    public const string processId = nameof(processId);

    public const string AddLogProcessWithDataSp = "[goldsync].[AddLogProcessWithData]";
}
