namespace MovieActorSearch.Application;

public sealed class MatchRequest
{
    public MatchRequest(string actor1, string actor2, bool moviesOnly)
    {
        Actor1 = actor1;
        Actor2 = actor2;
        MoviesOnly = moviesOnly;
    }

    public string Actor1 { get; }
    public string Actor2 { get; }
    public bool MoviesOnly { get; }
}