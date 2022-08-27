namespace MovieActorSearch.Application;

public sealed class MatchResponse
{
    public MatchResponse(IEnumerable<string> movies) => Movies = movies;

    public IEnumerable<string> Movies { get; }
}