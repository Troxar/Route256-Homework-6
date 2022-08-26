namespace MovieActorSearch.Application;

public sealed class MatchResult
{
    public IEnumerable<string> Movies { get; init; } = null!;
}