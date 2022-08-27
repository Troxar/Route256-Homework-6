using Dapper;
using Microsoft.Extensions.Options;
using MovieActorSearch.Domain;
using MovieActorSearch.Options;
using Npgsql;

namespace MovieActorSearch.Infrastructure.DbProvider;

public class PostgresDbProvider : IDbProvider
{
    private readonly DbOptions _dbOptions;

    public PostgresDbProvider(IOptions<DbOptions> dbOptions)
    {
        _dbOptions = dbOptions.Value;
    }
    
    public async Task<Actor?> FindActor(string name, CancellationToken ct)
    {
        await using var connection = new NpgsqlConnection(_dbOptions.ConnectionString);
        await connection.OpenAsync(ct);
        
        // todo: prevent sql-injection
        
        var query = $"select actor_id from actors where name='{name}';";
        string id = connection.ExecuteScalar<string>(query);
        
        return id is null ? null : new Actor(id, name);
    }
}