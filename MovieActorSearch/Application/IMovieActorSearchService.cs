namespace MovieActorSearch.Application;

public interface IMovieActorSearchService
{
    Task<MatchResult?> MatchActors(MatchRequest request, CancellationToken ct);
}