namespace MovieActorSearch.Application;

public sealed record MatchResponse(IEnumerable<string> Movies);