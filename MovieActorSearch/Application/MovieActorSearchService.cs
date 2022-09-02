using MovieActorSearch.Application.Utils;
using MovieActorSearch.Domain;
using MovieActorSearch.HttpClientApiProvider;
using MovieActorSearch.PostgreDbProvider;
using MovieActorSearch.PostgreDbProvider.Exceptions;

namespace MovieActorSearch.Application;

public class MovieActorSearchService : IMovieActorSearchService
{
    private const string RoleActor = "Actor";
    private const string RoleActress = "Actress";
    
    private readonly IDbProvider _dbProvider;
    private readonly IApiProvider _apiProvider;

    public MovieActorSearchService(IDbProvider dbProvider, IApiProvider apiProvider)
    {
        _dbProvider = dbProvider;
        _apiProvider = apiProvider;
    }
    
    public async Task<MatchResponse> MatchActors(MatchRequest request, CancellationToken ct)
    {
        var movies1 = await GetActorMovies(request.Actor1, request.MoviesOnly, ct);
        
        IEnumerable<Movie> commonMovies;
        if (request.Actor1 == request.Actor2)
        {
            commonMovies = movies1;
        }
        else
        {
            var movies2 = await GetActorMovies(request.Actor2, request.MoviesOnly, ct);
            commonMovies = MovieHelper.Intersect(movies1, movies2);
        }
        
        var result = commonMovies.Select(x => x.Title)
            .OrderBy(x => x);
        
        return new MatchResponse(result);
    }

    private async Task<Movie[]> GetActorMovies(string actorName, bool moviesOnly, CancellationToken ct)
    {
        var actor = await GetActor(actorName, ct);
        var movies = (await _apiProvider.FindActorMovies(actor.Id, ct))?.CastMovies;

        if (movies is null)
            return Array.Empty<Movie>();
        
        if (moviesOnly)
            movies = movies.Where(x => x.Role is RoleActor or RoleActress).ToArray();

        return movies;
    }
    
    private async Task<Actor> GetActor(string name, CancellationToken ct)
    {
        Actor actor;

        try
        {
            actor = await _dbProvider.FindActor(name, ct);
            return actor;
        }
        catch (ActorNotFoundException) { }

        actor = await _apiProvider.FindActor(name, ct);    
        await _dbProvider.SaveActor(actor, ct);
        
        return actor;
    }
}