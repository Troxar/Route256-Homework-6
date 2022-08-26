using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MovieActorSearch;
using Newtonsoft.Json;
using Npgsql;

namespace MovieActorSearch.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieActorSearchController: ControllerBase
{
    private readonly ConnectionConfigOptions _connectionsConfigOptions;

    public MovieActorSearchController([FromServices]IOptions<ConnectionConfigOptions> connectionsConfigOptions)
    {
        _connectionsConfigOptions = connectionsConfigOptions.Value;
    }
    
    [HttpPost]
    public async Task<IEnumerable<string>> Post(MatchActorsRequest request)
    {
        var result = new List<string>();

        var key = _connectionsConfigOptions.ImdbApiKey;

        var cs = _connectionsConfigOptions.DatabaseConnectionString;
        
        using var con = new NpgsqlConnection(cs);
        
        con.Open();
        var q1 = "select actor_id from actors where name='" + request.Actor1 + "';";
        var q2 = "select actor_id from actors where name='" + request.Actor2 + "';";

        string val1 = con.ExecuteScalar<string>(q1);
        string val2 = con.ExecuteScalar<string>(q2);

        var clnt = new HttpClient();

        if (val1 == null)
        {
            var c = await clnt.GetAsync("https://imdb-api.com/en/API/SearchName/" + key + "/"+ request.Actor1);
            var res = await c.Content.ReadAsStringAsync();
            var a = JsonConvert.DeserializeObject<MovieActors>(res);

            val1 = a.Results.FirstOrDefault(t => request.Actor1 == t.Title)?.Id;
        }

        if (val2 == null)
        {
            var c = await clnt.GetAsync("https://imdb-api.com/en/API/SearchName/" + key + "/" + request.Actor2);
            var res = await c.Content.ReadAsStringAsync();
            var a = JsonConvert.DeserializeObject<MovieActors>(res);

            val2 = a.Results.FirstOrDefault(t => request.Actor2 == t.Title)?.Id;
        }

        if (val1 != null && val2 != null)
        {
            var m1 = await clnt.GetAsync("https://imdb-api.com/en/API/Name/" + key + "/" + val1);
            var res1 = await m1.Content.ReadAsStringAsync();
            var movs1 = JsonConvert.DeserializeObject<ActorMovies>(res1).CastMovies;

            var m2 = await clnt.GetAsync("https://imdb-api.com/en/API/Name/" + key + "/" + val2);
            var res2 = await m2.Content.ReadAsStringAsync();
            var movs2 = JsonConvert.DeserializeObject<ActorMovies>(res2).CastMovies;

            // todo: Поиск только по фильмам MoviesOnly
            //if (request.MoviesOnly == true)
            //{
            //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
            //    movs1 = movs1.Where(m => m.Role == "Actress" || m.Role == "Actor").ToArray();
            //}

            foreach (var movies1 in movs1)
            {
                foreach (var movies2 in movs2)
                {
                    if (movies1.Id == movies2.Id)
                    {
                        result.Add(movies1.Title);
                    }
                }
            }
        }

        return result;
    }
}
