﻿using MovieActorSearch.Domain;

namespace MovieActorSearch.Infrastructure.DbProvider;

public interface IDbProvider
{
    Task<Actor?> FindActor(string name, CancellationToken ct);
}