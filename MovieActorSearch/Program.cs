using MovieActorSearch.Application;
using MovieActorSearch.Controllers;
using MovieActorSearch.Infrastructure.ApiProvider;
using MovieActorSearch.Infrastructure.DbProvider;
using MovieActorSearch.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMovieActorSearchService, MovieActorSearchService>();
builder.Services.AddTransient<IDbProvider, PostgresDbProvider>();
builder.Services.AddTransient<IApiProvider, HttpClientApiProvider>();
builder.Services.AddTransient<MovieActorSearchRequestValidator>();
builder.Services.AddOptions<DbOptions>().BindConfiguration(nameof(DbOptions));
builder.Services.AddOptions<ImdbOptions>().BindConfiguration(nameof(ImdbOptions));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();