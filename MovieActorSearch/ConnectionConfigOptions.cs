namespace MovieActorSearch;

public sealed class ConnectionConfigOptions
{
    public string DatabaseConnectionString { get; init; } = string.Empty;
    public string ImdbApiKey { get; init; } = string.Empty;
}