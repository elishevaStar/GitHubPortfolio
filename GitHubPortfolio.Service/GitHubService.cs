using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitHubPortfolio.Api.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Octokit;


namespace GitHubPortfolio.Service
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubSettings _settings;

        public GitHubService(IOptions<GitHubSettings> options)
        {
            _settings = options.Value;
            _client = new GitHubClient(new ProductHeaderValue("my-github-app"))
            {
                Credentials = new Credentials(_settings.Token)
            };
        }

        public async Task<int> GetUserFollowersAsync(string userName)
        {
            var user = await _client.User.Get(userName);
            return user.Followers;
        }

        public async Task<List<Repository>> SearchRepositoriesInCSharp()
        {
            var request = new SearchRepositoriesRequest("repo-name") { Language = Language.CSharp };
            var result = await _client.Search.SearchRepo(request);
            return result.Items.ToList();
        }
    }
}
