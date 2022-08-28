using Microsoft.Extensions.Options;
using MovieActorSearch.Domain;
using MovieActorSearch.Options;
using Npgsql;

namespace MovieActorSearch.Infrastructure.DbProvider;

public class PostgresDbProvider : IDbProvider
{
    private readonly DbOptions _dbOptions;
    private readonly ILogger<PostgresDbProvider> _logger;

    public PostgresDbProvider(IOptions<DbOptions> dbOptions, ILogger<PostgresDbProvider> logger)
    {
        _dbOptions = dbOptions.Value;
        _logger = logger;
    }
    
    public async Task<Actor?> FindActor(string name, CancellationToken ct)
    {
        await using var connection = new NpgsqlConnection(_dbOptions.ConnectionString);
        await connection.OpenAsync(ct);

        var query = "select imdb_id from actors where name = @name";
        await using var sqlCommand = new NpgsqlCommand(query, connection);
        sqlCommand.Parameters.AddWithValue("name", name);
        
        var id = (string?)await sqlCommand.ExecuteScalarAsync(ct);
        if (id is null)
        {
            _logger.LogInformation("Actor not found: {name}", name);
            return null;
        }
        
        _logger.LogInformation("Actor found: {name}", name);
        return new Actor(id, name);
    }

    public async Task SaveActor(Actor actor, CancellationToken ct)
    {
        await using var connection = new NpgsqlConnection(_dbOptions.ConnectionString);
        await connection.OpenAsync(ct);
        
        var query = "insert into actors (imdb_id, name) values (@id, @name)";
        await using var sqlCommand = new NpgsqlCommand(query, connection);
        sqlCommand.Parameters.AddWithValue("id", actor.Id);
        sqlCommand.Parameters.AddWithValue("name", actor.Title);
        
        await sqlCommand.ExecuteNonQueryAsync(ct);
        
        _logger.LogInformation("Actor saved: [{id}] {name}", actor.Id, actor.Title);
    }
}