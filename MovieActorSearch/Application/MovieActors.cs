using MovieActorSearch.Domain;

namespace MovieActorSearch.Application;

public sealed record MovieActors
{
    public Actor[] Results { get; set; } = Array.Empty<Actor>();
}