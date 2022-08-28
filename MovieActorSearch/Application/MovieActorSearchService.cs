using MovieActorSearch.Application.Exceptions;
using MovieActorSearch.Domain;
using MovieActorSearch.Infrastructure.ApiProvider;
using MovieActorSearch.Infrastructure.DbProvider;

namespace MovieActorSearch.Application;

public class MovieActorSearchService : IMovieActorSearchService
{
    private readonly IDbProvider _dbProvider;
    private readonly IApiProvider _apiProvider;

    public MovieActorSearchService(IDbProvider dbProvider, IApiProvider apiProvider)
    {
        _dbProvider = dbProvider;
        _apiProvider = apiProvider;
    }
    
    public async Task<MatchResponse> MatchActors(MatchRequest request, CancellationToken ct)
    {
        var movies1 = await GetActorMovies(request.Actor1, ct);
        
        IEnumerable<Movie> commonMovies;
        if (request.Actor1 == request.Actor2)
        {
            commonMovies = movies1;
        }
        else
        {
            var movies2 = await GetActorMovies(request.Actor2, ct);
            commonMovies = Movie.Intersect(movies1, movies2);
        }
        
        // todo: search MoviesOnly
        //if (request.MoviesOnly == true)
        //{
        //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
        //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
        //}

        var result = commonMovies.Select(x => x.Title)
            .OrderBy(x => x);
        
        return new MatchResponse(result);
    }

    private async Task<Movie[]> GetActorMovies(string actorName, CancellationToken ct)
    {
        var actor = await GetActor(actorName, ct);
        var movies = await _apiProvider.FindActorMovies(actor.Id, ct);
        
        return movies is null ? Array.Empty<Movie>() : movies.CastMovies;
    }
    
    private async Task<Actor> GetActor(string name, CancellationToken ct)
    {
        var actor = await _dbProvider.FindActor(name, ct);
        if (actor is not null)
            return actor;

        actor = await _apiProvider.FindActor(name, ct);
        if (actor is null)
            throw new ActorNotFoundException($"Actor not found: {name}");
        
        await _dbProvider.SaveActor(actor, ct);
        
        return actor;
    }
}