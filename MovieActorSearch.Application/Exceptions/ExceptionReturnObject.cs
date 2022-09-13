namespace MovieActorSearch.Application.Exceptions;

internal record ExceptionReturnObject(string Error)
{
    public override string ToString()
    {
        return $"{{ Error = {Error} }}";
    }
}