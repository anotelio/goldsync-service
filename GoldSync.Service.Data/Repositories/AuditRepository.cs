using System.Data;
using GoldSync.Service.Contracts.Defaults;
using GoldSync.Service.Contracts.Enums;
using GoldSync.Service.Data.Contexts;
using Dapper;

namespace GoldSync.Service.Data.Repositories;

public class AuditRepository
{
    private readonly CubDbContext cubDbContext;

    public AuditRepository(CubDbContext cubDbContext)
    {
        this.cubDbContext = cubDbContext;
    }

    public async Task<int> AddLogProcess(Source source, ProcessStatus processStatus)
    {
        var parameters = new
        {
            sourceId = source,
            processStatusId = processStatus
        };

        DynamicParameters p = new();

        p.AddDynamicParams(parameters);
        p.Add(SqlExecutionsDefault.processId,
            dbType: DbType.Int32,
            direction: ParameterDirection.Output);

        CommandDefinition command = new(
            commandText: SqlExecutionsDefault.AddLogProcessSp,
            parameters: p,
            commandType: CommandType.StoredProcedure);

        using var connection = this.cubDbContext.CreateConnection();
        await connection.ExecuteAsync(command);

        return p.Get<int>(SqlExecutionsDefault.processId);
    }

    public async Task AddLogProcessWithData(int processId, string processData)
    {
        var parameters = new
        {
            processId,
            processData
        };

        DynamicParameters p = new();

        p.AddDynamicParams(parameters);

        CommandDefinition command = new(
            commandText: SqlExecutionsDefault.AddLogProcessWithDataSp,
            parameters: p,
            commandType: CommandType.StoredProcedure);

        using var connection = this.cubDbContext.CreateConnection();
        await connection.ExecuteAsync(command);
    }
}
