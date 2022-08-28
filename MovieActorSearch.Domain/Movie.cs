namespace MovieActorSearch.Domain;

public sealed record Movie(string Id, string Title, string Role)
{
    public static IEnumerable<Movie> Intersect(IEnumerable<Movie> movies1, IEnumerable<Movie> movies2)
    {
        return movies1.Intersect(movies2, new MovieComparer());
    }    
}