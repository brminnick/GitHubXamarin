using System.Collections.Generic;
using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class RepositoryConnection
    {
        public RepositoryConnection(IEnumerable<Repository> nodes, PageInfo pageInfo) =>
            (RepositoryList, PageInfo) = (nodes, pageInfo);

        [JsonProperty("nodes")]
        public IEnumerable<Repository> RepositoryList { get; }

        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; }
    }
}
