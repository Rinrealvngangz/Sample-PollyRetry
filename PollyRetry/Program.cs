using Microsoft.Net.Http.Headers;
using PollyRetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.ConfigureServices((_, services) =>
{
    services.AddSingleton<IGithubService>(x => new GithubService(x.GetService<IHttpClientFactory>()));
    services.AddHttpClient("GitHub", client =>
    {
        client.BaseAddress = new Uri("https://api.github.com/");
        // using Microsoft.Net.Http.Headers;
        // The GitHub API requires two headers.
        client.DefaultRequestHeaders.Add(
            HeaderNames.Accept, "application/vnd.github.v3+json"); 
        client.DefaultRequestHeaders.Add(
            HeaderNames.UserAgent, "HttpRequestsSample");
    });
});
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