namespace MovieActorSearch;

public sealed class MatchActorsRequest
{
    public string Actor1 { get; set; } = "";
    public string Actor2 { get; set; } = "";
    public bool MoviesOnly { get; set; }
}