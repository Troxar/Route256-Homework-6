using Microsoft.AspNetCore.Mvc;
using MovieActorSearch.Application.Abstractions;
using MovieActorSearch.Application.Exceptions;
using MovieActorSearch.Application.Services;
using MovieActorSearch.WebUI.Requests;
using MovieActorSearch.WebUI.Responses;

namespace MovieActorSearch.WebUI.Controllers;

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
        var result = await _searchService.MatchActors(new MatchRequest(
            request.Actor1,
            request.Actor2,
            request.MoviesOnly
        ), ct);

        return new MovieActorSearchResponse(result.Movies);
    }
}