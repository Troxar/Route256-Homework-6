using MovieActorSearch.Domain;

namespace MovieActorSearch.Application.Utils;

internal class MovieComparer : IEqualityComparer<Movie>
{
    public bool Equals(Movie? x, Movie? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        return x.Id == y.Id;
    }

    public int GetHashCode(Movie obj)
    {
        return obj.Id.GetHashCode();
    }
}