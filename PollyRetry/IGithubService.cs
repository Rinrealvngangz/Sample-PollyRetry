namespace PollyRetry;

public interface IGithubService
{
    Task<GithubUser> GetProfileByUserName(string userName);
    Task<List<GithubUser>> GetOrgByName(string orgsName);
}