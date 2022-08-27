namespace MovieActorSearch.Application;

public interface IMovieActorSearchService
{
    Task<MatchResponse> MatchActors(MatchRequest request, CancellationToken ct);
}