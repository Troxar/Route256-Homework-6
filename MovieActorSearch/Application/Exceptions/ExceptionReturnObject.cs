namespace MovieActorSearch.Application.Exceptions;

public record ExceptionReturnObject(string Error)
{
    public override string ToString()
    {
        return $"{{ Error = {Error} }}";
    }
}