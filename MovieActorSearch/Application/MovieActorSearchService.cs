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
    
    public async Task<MatchResult?> MatchActors(MatchRequest request, CancellationToken ct)
    {
        var actor1 = await _dbProvider.FindActor(request.Actor1, ct);
        if (actor1 is null)
            actor1 = await _apiProvider.FindActor(request.Actor1, ct);
        
        // todo: throw exception
        
        if (actor1 is null)
            return null;
        
        var movies1 = await _apiProvider.FindActorMovies(actor1!.Id, ct);
        if (movies1 is null)
            movies1 = new ActorMovies(Array.Empty<Movie>());

        var actor2 = await _dbProvider.FindActor(request.Actor2, ct);
        if (actor2 is null)
            actor2 = await _apiProvider.FindActor(request.Actor2, ct);
        
        // todo: throw exception
        
        if (actor2 is null)
            return null;
        
        var movies2 = await _apiProvider.FindActorMovies(actor2!.Id, ct);
        if (movies2 is null)
            movies2 = new ActorMovies(Array.Empty<Movie>());

        // todo: search MoviesOnly
        //if (request.MoviesOnly == true)
        //{
        //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
        //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
        //}

        var result = movies1.CastMovies.Intersect(movies2.CastMovies, new MovieComparer())
            .Select(x => x.Title)
            .OrderBy(x => x);
        
        return await Task.FromResult(new MatchResult { Movies = result });
    }
}