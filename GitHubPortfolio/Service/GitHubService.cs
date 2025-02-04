using GitHubPortfolio.Api.Configurations;
using Microsoft.Extensions.Options;
using Octokit;

namespace GitHubPortfolio.Api.Service
{
    public class GitHubService:IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubSettings _settings;

        public GitHubService(IOptions<GitHubSettings> options)
        {
            _settings = options.Value;
            _client = new GitHubClient(new ProductHeaderValue("my-github-app"))
            {
                Credentials = new Credentials(_settings.UserName, _settings.Token)
            };
        }

        public async Task<List<RepositoryDto>> GetPortfolio(string userName)
        {
            var repositories = await _client.Repository.GetAllForUser(userName);
            return repositories.Select(repo => new RepositoryDto
            {
                Name = repo.Name,
                HtmlUrl = repo.HtmlUrl,
                Language = repo.Language,
                StargazersCount = repo.StargazersCount,
                OpenIssuesCount = repo.OpenIssuesCount,
                PushedAt = repo.PushedAt
            }).ToList();
        }

        public async Task<List<RepositoryDto>> SearchRepositories(string? repoName = null, string? language = null, string? userName = null)
        {
            string query = "";

            if (!string.IsNullOrEmpty(repoName))
            {
                query += repoName + " ";
            }

            if (!string.IsNullOrEmpty(language))
            {
                query += $"language:{language} ";
            }

            if (!string.IsNullOrEmpty(userName))
            {
                query += $"user:{userName} ";
            }

            var request = new SearchRepositoriesRequest(query.Trim())
            {
                In = new[] { InQualifier.Name }
            };

            var result = await _client.Search.SearchRepo(request);
            return result.Items.Select(repo => new RepositoryDto
            {
                Name = repo.Name,
                HtmlUrl = repo.HtmlUrl,
                Language = repo.Language,
                StargazersCount = repo.StargazersCount,
                OpenIssuesCount = repo.OpenIssuesCount,
                PushedAt = repo.PushedAt
            }).ToList();
        }

    }
}
