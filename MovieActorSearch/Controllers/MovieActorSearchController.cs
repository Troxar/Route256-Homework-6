using Microsoft.AspNetCore.Mvc;
using MovieActorSearch.Application;
using MovieActorSearch.Application.Exceptions;

namespace MovieActorSearch.Controllers;

[ApiController]
[Route("[controller]")]
[AppExceptionFilter]
public class MovieActorSearchController: ControllerBase
{
    private readonly IMovieActorSearchService _searchService;

    public MovieActorSearchController([FromServices]IMovieActorSearchService searchService)
    {
        _searchService = searchService;
    }
    
    [HttpPost]
    public async Task<MovieActorSearchResponse> MatchActors(MovieActorSearchRequest request, CancellationToken ct)
    {
        // todo: request validation

        var result = await _searchService.MatchActors(new MatchRequest(
            actor1: request.Actor1,
            actor2: request.Actor2,
            moviesOnly: request.MoviesOnly
        ), ct);

        return new MovieActorSearchResponse(result.Movies);
    }
}
