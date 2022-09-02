using Microsoft.Extensions.Options;
using MovieActorSearch.Domain;
using Newtonsoft.Json;

namespace MovieActorSearch.HttpClientApiProvider;

public class HttpClientApiProvider : IApiProvider
{
    private readonly ImdbOptions _imdbOptions;
    private readonly HttpClient _httpClient;

    public HttpClientApiProvider(IOptions<ImdbOptions> imdbOptions, HttpClient httpClient)
    {
        _imdbOptions = imdbOptions.Value;
        _httpClient = httpClient;
    }
    
    public async Task<Actor?> FindActor(string name, CancellationToken ct)
    {
        var requestUri = $"{_imdbOptions.SearchNameUri}/{_imdbOptions.Key}/{name}";

        HttpResponseMessage? response;
        
        try
        {
            response = await _httpClient.GetAsync(requestUri, ct);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }

        if (response is null)
            throw new NullReferenceException("Null reference: " + nameof(response));
        
        var result = await response.Content.ReadAsStringAsync(ct);
        var actors = JsonConvert.DeserializeObject<MovieActors>(result);

        return actors?.Results.FirstOrDefault(t => name == t.Title);
    }

    public async Task<ActorMovies?> FindActorMovies(string id, CancellationToken ct)
    {
        var requestUri = $"{_imdbOptions.SearchActorMoviesUri}/{_imdbOptions.Key}/{id}";
        var response = await _httpClient.GetAsync(requestUri, ct);
        var result = await response.Content.ReadAsStringAsync(ct);
        
        return JsonConvert.DeserializeObject<ActorMovies>(result);
    }
}