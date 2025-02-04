using Dapper;
using System.Data;

public class DapperRepository
{
    private readonly DatabaseContext _dbContext;

    public DapperRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
    {
        using var connection = _dbContext.CreateConnection();
        return await connection.QueryAsync<T>(sql, param);
    }

    public async Task<int> ExecuteAsync(string sql, object param = null)
    {
        using var connection = _dbContext.CreateConnection();
        return await connection.ExecuteAsync(sql, param);
    }
}
