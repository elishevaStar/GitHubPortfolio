namespace GitHubPortfolio.Api
{
    public class RepositoryDto
    {
        public string Name { get; set; }
        public string HtmlUrl { get; set; }
        public string Language { get; set; }
        public int StargazersCount { get; set; }
        public int OpenIssuesCount { get; set; }
        public DateTimeOffset? PushedAt { get; set; }
    }
}
