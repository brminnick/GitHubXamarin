using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class RepositoryResponse
    {
        public RepositoryResponse(Repository repository) => Repository = repository;

        [JsonProperty("repository")]
        public Repository Repository { get; }
    }

}
