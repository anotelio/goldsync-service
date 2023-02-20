using System.Data;
using System.Data.SqlClient;
using GoldSync.Service.Contracts.Options;
using Microsoft.Extensions.Options;

namespace GoldSync.Service.Data.Contexts;

public sealed class CubDbContext
{
    private readonly string cubDbConnectionString;

    public CubDbContext(IOptions<DbSettings> dbOptions)
    {
        this.cubDbConnectionString = dbOptions.Value.ConnectionStrings.CubDb;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(this.cubDbConnectionString);
    }
}
