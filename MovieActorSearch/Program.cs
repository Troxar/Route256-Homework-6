using FluentValidation;
using FluentValidation.AspNetCore;
using MovieActorSearch.Application;
using MovieActorSearch.Controllers;
using MovieActorSearch.HttpClientApiProvider;
using MovieActorSearch.PostgreDbProvider;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMovieActorSearchService, MovieActorSearchService>();
builder.Services.AddTransient<IDbProvider, PostgreDbProvider>();
builder.Services.AddTransient<IApiProvider, HttpClientApiProvider>();
builder.Services.AddOptions<DbOptions>().BindConfiguration(nameof(DbOptions));
builder.Services.AddOptions<ImdbOptions>().BindConfiguration(nameof(ImdbOptions));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<MovieActorSearchRequest>, MovieActorSearchRequestValidator>();

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