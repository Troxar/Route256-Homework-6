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
    
    public async Task<MatchResult> MatchActors(MatchRequest request, CancellationToken ct)
    {
        var movies1 = await GetActorMovies(request.Actor1, ct);
        var movies2 = await GetActorMovies(request.Actor2, ct);

        // todo: search MoviesOnly
        //if (request.MoviesOnly == true)
        //{
        //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
        //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
        //}

        var result = movies1.Intersect(movies2, new MovieComparer())
            .Select(x => x.Title)
            .OrderBy(x => x);
        
        return await Task.FromResult(new MatchResult { Movies = result });
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
        
        if (actor is null)
            actor = await _apiProvider.FindActor(name, ct);

        if (actor is null)
            throw new ActorNotFoundException($"Actor not found: {name}");

        return actor;
    }
}