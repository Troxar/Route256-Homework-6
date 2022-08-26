namespace MovieActorSearch;

public sealed record MovieActors
{
    public Actor[] Results { get; set; } = Array.Empty<Actor>();
}