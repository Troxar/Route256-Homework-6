namespace MovieActorSearch.Controllers;

public sealed class MovieActorSearchRequest
{
    public string Actor1 { get; init; } = "";
    public string Actor2 { get; init; } = "";
    public bool MoviesOnly { get; init; }
}