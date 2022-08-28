namespace MovieActorSearch.Controllers;

public sealed record MovieActorSearchRequest(string Actor1, string Actor2, bool MoviesOnly);