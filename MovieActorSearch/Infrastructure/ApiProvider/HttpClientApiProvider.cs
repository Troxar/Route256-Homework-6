using Microsoft.Extensions.Options;
using MovieActorSearch.Domain;
using MovieActorSearch.Options;
using Newtonsoft.Json;

namespace MovieActorSearch.Infrastructure.ApiProvider;

public class HttpClientApiProvider : IApiProvider
{
    private readonly ImdbOptions _imdbOptions;

    public HttpClientApiProvider(IOptions<ImdbOptions> imdbOptions)
    {
        _imdbOptions = imdbOptions.Value;
    }
    
    public async Task<Actor?> FindActor(string name, CancellationToken ct)
    {
        using var client = new HttpClient();
        
        var requestUri = $"{_imdbOptions.SearchNameUri}/{_imdbOptions.Key}/{name}";
        var response = await client.GetAsync(requestUri, ct);
        var result = await response.Content.ReadAsStringAsync(ct);
        var actors = JsonConvert.DeserializeObject<MovieActors>(result);

        return actors?.Results.FirstOrDefault(t => name == t.Title);
    }

    public async Task<ActorMovies?> FindActorMovies(string id, CancellationToken ct)
    {
        using var client = new HttpClient();
        
        var requestUri = $"{_imdbOptions.SearchActorMoviesUri}/{_imdbOptions.Key}/{id}";
        var response = await client.GetAsync(requestUri, ct);
        var result = await response.Content.ReadAsStringAsync(ct);
        
        return JsonConvert.DeserializeObject<ActorMovies>(result);
    }
}