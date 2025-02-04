using GitHubPortfolio.Api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GitHubPortfolio.Api.Controllers
{
    [Route("api/github")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        
        [HttpGet("portfolio/{userName}")]
        public async Task<IActionResult> GetPortfolio(string userName)
        {
            var repositories = await _gitHubService.GetPortfolio(userName);
            if (repositories == null || repositories.Count == 0)
            {
                return NotFound("No repositories found.");
            }
            return Ok(repositories);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchRepositories([FromQuery] string? repoName = null, [FromQuery] string? language = null, [FromQuery] string? userName = null)
        {
            var repositories = await _gitHubService.SearchRepositories(repoName, language, userName);
            if (repositories == null || repositories.Count == 0)
            {
                return NotFound("No repositories matched your search.");
            }
            return Ok(repositories);
        }
    }
}
