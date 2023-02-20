using System.Data.SqlClient;
using GoldSync.Service.Data.Contexts;

namespace GoldSync.Service.Data.Repositories;

public class CubDbBaseRepository
{
    private readonly CubDbContext cubDbContext;

    public CubDbBaseRepository(CubDbContext cubDbContext)
    {
        this.cubDbContext = cubDbContext;
    }

    public bool IsDbConnected()
    {
        using var connection = this.cubDbContext.CreateConnection();

        try
        {
            connection.Open();
            return true;
        }
        catch (SqlException)
        {
            return false;
        }
    }
}
