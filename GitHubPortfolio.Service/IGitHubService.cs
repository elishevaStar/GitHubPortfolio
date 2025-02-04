using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubPortfolio.Service
{
    public interface IGitHubService
    {
        Task<int> GetUserFollowersAsync(string userName);
        Task<List<Repository>> SearchRepositoriesInCSharp();
    }
}
