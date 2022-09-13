namespace MovieActorSearch.Application.Exceptions;

public class ActorNotFoundException : Exception
{
    public ActorNotFoundException(string message) : base(message) { }
}