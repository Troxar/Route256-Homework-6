using Dapper;
using Microsoft.Extensions.Options;
using MovieActorSearch.Domain;
using Newtonsoft.Json;
using Npgsql;

namespace MovieActorSearch.Application;

public class MovieActorSearchService : IMovieActorSearchService
{
    private readonly ConnectionConfigOptions _connectionsConfigOptions;

    public MovieActorSearchService(IOptions<ConnectionConfigOptions> connectionsConfigOptions)
    {
        _connectionsConfigOptions = connectionsConfigOptions.Value;
    }
    
    public async Task<MatchResult> MatchActors(MatchRequest request, CancellationToken ct)
    {
        var result = new List<string>();
        
        await using var connection = new NpgsqlConnection(_connectionsConfigOptions.DatabaseConnectionString);
        await connection.OpenAsync(ct);
        
        var queryActor1 = "select actor_id from actors where name='" + request.Actor1 + "';";
        var queryActor2 = "select actor_id from actors where name='" + request.Actor2 + "';";

        string actor1_id = connection.ExecuteScalar<string>(queryActor1);
        string actor2_id = connection.ExecuteScalar<string>(queryActor2);

        using var client = new HttpClient();
        var key = _connectionsConfigOptions.ImdbApiKey;

        if (actor1_id == null)
        {
            var response1 = await client.GetAsync("https://imdb-api.com/en/API/SearchName/" + key + "/"+ request.Actor1, ct);
            var result1 = await response1.Content.ReadAsStringAsync(ct);
            var actors1 = JsonConvert.DeserializeObject<MovieActors>(result1);

            actor1_id = actors1.Results.FirstOrDefault(t => request.Actor1 == t.Title)?.Id;
        }

        if (actor2_id == null)
        {
            var response2 = await client.GetAsync("https://imdb-api.com/en/API/SearchName/" + key + "/" + request.Actor2);
            var result2 = await response2.Content.ReadAsStringAsync(ct);
            var actors2 = JsonConvert.DeserializeObject<MovieActors>(result2);

            actor2_id = actors2.Results.FirstOrDefault(t => request.Actor2 == t.Title)?.Id;
        }

        if (actor1_id != null && actor2_id != null)
        {
            var response1 = await client.GetAsync("https://imdb-api.com/en/API/Name/" + key + "/" + actor1_id, ct);
            var result1 = await response1.Content.ReadAsStringAsync(ct);
            var movies1 = JsonConvert.DeserializeObject<ActorMovies>(result1).CastMovies;

            var response2 = await client.GetAsync("https://imdb-api.com/en/API/Name/" + key + "/" + actor2_id, ct);
            var result2 = await response2.Content.ReadAsStringAsync(ct);
            var movies2 = JsonConvert.DeserializeObject<ActorMovies>(result2).CastMovies;

            // todo: search MoviesOnly
            //if (request.MoviesOnly == true)
            //{
            //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
            //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
            //}
            
            foreach (var movie1 in movies1)
            {
                foreach (var movie2 in movies2)
                {
                    if (movie1.Id == movie2.Id)
                    {
                        result.Add(movie1.Title);
                    }
                }
            }
        }
        
        return await Task.FromResult(new MatchResult { Movies = result });
    }
}