namespace MovieActorSearch.Application.Exceptions;

internal class ActorNotFoundException : Exception
{
    public ActorNotFoundException(string message) : base(message) { }
}