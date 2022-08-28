using MovieActorSearch.Domain;

namespace MovieActorSearch.PostgreDbProvider;

public interface IDbProvider
{
    Task<Actor?> FindActor(string name, CancellationToken ct);
    Task SaveActor(Actor actor, CancellationToken ct);
}