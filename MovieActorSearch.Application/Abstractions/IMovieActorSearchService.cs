using MovieActorSearch.Application.Services;

namespace MovieActorSearch.Application.Abstractions;

public interface IMovieActorSearchService
{
    Task<MatchResponse> MatchActors(MatchRequest request, CancellationToken ct);
}