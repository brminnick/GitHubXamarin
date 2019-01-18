using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class GitHubRepositoryResponse
    {
        public GitHubRepositoryResponse(GitHubRepository repository) => Repository = repository;

        [JsonProperty("repository")]
        public GitHubRepository Repository { get; }
    }

}
