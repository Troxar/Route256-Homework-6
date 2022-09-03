using MovieActorSearch.Domain;

namespace MovieActorSearch.Application.Abstractions;

public interface IApiProvider
{
    Task<Actor> FindActor(string name, CancellationToken ct);
    Task<ActorMovies?> FindActorMovies(string id, CancellationToken ct);
}