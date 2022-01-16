using System.Net;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace PollyRetry;

public class GithubService : IGithubService
{
    private const int MAX_ENTRIES = 3;
    private static readonly Random Random =new Random();
    private readonly IHttpClientFactory _clientFactory;
    private readonly AsyncRetryPolicy _retryPolicy;
    public GithubService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _retryPolicy =  Policy.Handle<HttpRequestException>(exception =>
        {
            return exception.Message != "This is a fake request exception";
        }).WaitAndRetryAsync(MAX_ENTRIES,times => TimeSpan.FromMilliseconds(times *100));
    }

    public async Task<GithubUser> GetProfileByUserName(string userName)
    {
        var client = _clientFactory.CreateClient("GitHub");
       return await _retryPolicy.ExecuteAsync(async () =>
        {
            if (Random.Next(1, 3) == 1)
                throw new HttpRequestException("This is a fake request exception");
            

            var result = await client.GetAsync($"users/{userName}");
            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            var resultString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GithubUser>(resultString);
        });
    }
    
    public async Task<List<GithubUser>> GetOrgByName(string orgsName)
    {
        var client = _clientFactory.CreateClient("GitHub");
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var result = await client.GetAsync($"orgs/{orgsName}");
            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            var resultString = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<GithubUser>>(resultString);
        });
    }

}