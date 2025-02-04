using GitHubPortfolio.Api.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Validations;
using System.Collections.Generic;
using System.Text.Json;

namespace GitHubPortfolio.Api.CachedSevices
{
    public class ChachedGithubService : IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;
        private const string UserProtfolioKey = "UserProtfolioKey";
        private const string LastUpdateKey = "LastUpdateKey";

        public ChachedGithubService(IGitHubService gitHubService, IMemoryCache memoryCache, HttpClient httpClient)
        {
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
            _httpClient = httpClient;
        }
        public async Task<List<RepositoryDto>> GetPortfolio(string userName)
        {
            if (_memoryCache.TryGetValue(UserProtfolioKey, out List <RepositoryDto> repositoryDto) &&
                _memoryCache.TryGetValue(LastUpdateKey, out DateTime lastCachedTime))
            {
                DateTime? lastUpdate = await GetLastUpdatedTime(userName);

                if (lastUpdate.HasValue && lastUpdate > lastCachedTime)
                {
                    _memoryCache.Remove(UserProtfolioKey);
                    _memoryCache.Remove(LastUpdateKey);
                }
                else
                {
                    return repositoryDto;
                }
            }

            repositoryDto = await _gitHubService.GetPortfolio(userName);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _memoryCache.Set(UserProtfolioKey, repositoryDto, cacheOptions);
            _memoryCache.Set(LastUpdateKey, DateTime.UtcNow, cacheOptions);

            return repositoryDto;
        }

        private async Task<DateTime?> GetLastUpdatedTime(string userName)
        {
            var url = $"https://api.github.com/users/{userName}/events/public";
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "CSharp-HttpClient");

            using var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var events = JsonSerializer.Deserialize<List<GithubEvent>>(json);

            return events?.FirstOrDefault()?.CreatedAt;
        }
        private class GithubEvent
        {
            public DateTime CreatedAt { get; set; }
        }
        public Task<List<RepositoryDto>> SearchRepositories(string? repoName = null, string? language = null, string? userName = null)
        {
            return _gitHubService.SearchRepositories(repoName, language, userName);
        }
    }
}
