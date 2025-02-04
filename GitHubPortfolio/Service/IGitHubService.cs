using Octokit;

namespace GitHubPortfolio.Api.Service
{
    public interface IGitHubService
    {
        Task<List<RepositoryDto>> GetPortfolio(string userName);
        Task<List<RepositoryDto>> SearchRepositories(string? repoName = null, string? language = null, string? userName = null);
    }
}
