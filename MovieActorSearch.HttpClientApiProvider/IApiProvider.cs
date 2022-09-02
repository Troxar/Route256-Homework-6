using MovieActorSearch.Domain;

namespace MovieActorSearch.HttpClientApiProvider;

public interface IApiProvider
{
    Task<Actor> FindActor(string name, CancellationToken ct);
    Task<ActorMovies?> FindActorMovies(string id, CancellationToken ct);
}