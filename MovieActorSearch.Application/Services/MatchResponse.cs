namespace MovieActorSearch.Application.Services;

public sealed record MatchResponse(IEnumerable<string> Movies);