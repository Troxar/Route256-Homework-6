namespace MovieActorSearch.Application.Services;

public sealed record MatchRequest(string Actor1, string Actor2, bool MoviesOnly);