using Microsoft.Extensions.Options;
using MovieActorSearch.Domain;
using MovieActorSearch.Infrastructure.DbProvider;
using MovieActorSearch.Options;
using Newtonsoft.Json;

namespace MovieActorSearch.Application;

public class MovieActorSearchService : IMovieActorSearchService
{
    private readonly IDbProvider _dbProvider;
    private readonly ImdbOptions _options;

    public MovieActorSearchService(IDbProvider dbProvider, IOptions<ImdbOptions> options)
    {
        _dbProvider = dbProvider;
        _options = options.Value;
    }
    
    public async Task<MatchResult> MatchActors(MatchRequest request, CancellationToken ct)
    {
        var result = new List<string>();

        var actor1 = await _dbProvider.FindActor(request.Actor1, ct);
        var actor2 = await _dbProvider.FindActor(request.Actor2, ct);
        
        using var client = new HttpClient();
        var key = _options.Key;

        if (actor1 is null)
        {
            var response1 = await client.GetAsync("https://imdb-api.com/en/API/SearchName/" + key + "/"+ request.Actor1, ct);
            var result1 = await response1.Content.ReadAsStringAsync(ct);
            var actors1 = JsonConvert.DeserializeObject<MovieActors>(result1);

            actor1 = actors1.Results.FirstOrDefault(t => request.Actor1 == t.Title);
        }

        if (actor2 is null)
        {
            var response2 = await client.GetAsync("https://imdb-api.com/en/API/SearchName/" + key + "/" + request.Actor2);
            var result2 = await response2.Content.ReadAsStringAsync(ct);
            var actors2 = JsonConvert.DeserializeObject<MovieActors>(result2);

            actor2 = actors2.Results.FirstOrDefault(t => request.Actor2 == t.Title);
        }

        if (actor1 is not null && actor2 is not null)
        {
            var response1 = await client.GetAsync("https://imdb-api.com/en/API/Name/" + key + "/" + actor1.Id, ct);
            var result1 = await response1.Content.ReadAsStringAsync(ct);
            var movies1 = JsonConvert.DeserializeObject<ActorMovies>(result1).CastMovies;

            var response2 = await client.GetAsync("https://imdb-api.com/en/API/Name/" + key + "/" + actor2.Id, ct);
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