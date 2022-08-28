namespace MovieActorSearch.Application;

public sealed record MatchRequest(string Actor1, string Actor2, bool MoviesOnly);