namespace GoldSync.Service.Contracts.Options;

public sealed class DbSettings
{
    public const string cubDbName = nameof(DbSettings.ConnectionStrings.CubDb);

    public ConnectionStringsValues ConnectionStrings { get; set; }

    public class ConnectionStringsValues
    {
        public string CubDb { get; set; }
    }

    public void Validate()
    {
        ArgumentNullException.ThrowIfNull(ConnectionStrings);

        if (string.IsNullOrEmpty(ConnectionStrings.CubDb))
        {
            throw new ArgumentException($"{nameof(ConnectionStrings.CubDb)} is not configured correctly.");
        }
    }
}
