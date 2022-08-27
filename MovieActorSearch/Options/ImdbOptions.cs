namespace MovieActorSearch.Options;

public sealed class ImdbOptions
{
    public string Key { get; init; } = string.Empty;
    public string SearchNameUri { get; init; } = string.Empty;
    public string SearchActorMoviesUri { get; init; } = string.Empty;
}