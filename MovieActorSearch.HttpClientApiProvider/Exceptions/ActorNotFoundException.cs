﻿namespace MovieActorSearch.HttpClientApiProvider.Exceptions;

public class ActorNotFoundException : Exception
{
    public ActorNotFoundException(string message) : base(message) { }
}