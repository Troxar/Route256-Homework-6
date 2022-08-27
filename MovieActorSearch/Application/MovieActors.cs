using MovieActorSearch.Domain;

namespace MovieActorSearch.Application;

public sealed record MovieActors
{
    public Actor[] Results { get; init; } = Array.Empty<Actor>();
}