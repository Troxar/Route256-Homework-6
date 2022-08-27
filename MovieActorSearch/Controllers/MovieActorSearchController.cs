using Microsoft.AspNetCore.Mvc;
using MovieActorSearch.Application;

namespace MovieActorSearch.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieActorSearchController: ControllerBase
{
    private readonly IMovieActorSearchService _searchService;

    public MovieActorSearchController([FromServices]IMovieActorSearchService searchService)
    {
        _searchService = searchService;
    }
    
    [HttpPost]
    public async Task<IEnumerable<string>> Post(MovieActorSearchRequest request, CancellationToken ct)
    {
        // todo: request validation

        var result = await _searchService.MatchActors(new MatchRequest
        {
            Actor1 = request.Actor1,
            Actor2 = request.Actor2,
            MoviesOnly = request.MoviesOnly
        }, ct);

        // todo: handle exceptions
        
        return result is null ? Array.Empty<string>() : result.Movies;
    }
}
