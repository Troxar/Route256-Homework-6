using MovieActorSearch.Domain;

namespace MovieActorSearch.Application.Abstractions;

public interface IDbProvider
{
    Task<Actor> FindActor(string name, CancellationToken ct);
    Task SaveActor(Actor actor, CancellationToken ct);
}