namespace MovieActorSearch.Controllers;

public sealed class MovieActorSearchResponse
{
    public MovieActorSearchResponse(IEnumerable<string> movies) => Movies = movies;

    public IEnumerable<string> Movies { get; }
}