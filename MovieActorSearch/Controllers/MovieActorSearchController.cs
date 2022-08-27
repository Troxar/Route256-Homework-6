using FluentValidation;
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
    private readonly MovieActorSearchRequestValidator _validator;

    public MovieActorSearchController([FromServices]IMovieActorSearchService searchService, 
        MovieActorSearchRequestValidator validator)
    {
        _searchService = searchService;
        _validator = validator;
    }
    
    [HttpPost]
    public async Task<MovieActorSearchResponse> MatchActors(MovieActorSearchRequest request, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(request, ct);

        var result = await _searchService.MatchActors(new MatchRequest(
            actor1: request.Actor1,
            actor2: request.Actor2,
            moviesOnly: request.MoviesOnly
        ), ct);

        return new MovieActorSearchResponse(result.Movies);
    }
}