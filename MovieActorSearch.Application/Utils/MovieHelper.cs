using MovieActorSearch.Domain;

namespace MovieActorSearch.Application.Utils;

internal static class MovieHelper
{
    internal static IEnumerable<Movie> Intersect(IEnumerable<Movie> movies1, IEnumerable<Movie> movies2)
    {
        return movies1.Intersect(movies2, new MovieComparer());
    }   
}