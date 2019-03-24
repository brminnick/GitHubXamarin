using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class GitHubRepositoryConnectionResponse
    {
        public GitHubRepositoryConnectionResponse(RepositoryConnection repositoryConnection) =>
            RepositoryConnection = repositoryConnection;

        [JsonProperty("user")]
        public RepositoryConnection RepositoryConnection { get; }
    }
}
