using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class RepositoryConnection
    {
        public RepositoryConnection(List<GitHubRepository> repositoryList, PageInfo pageInfo) =>
            (RepositoryList, PageInfo) = (repositoryList, pageInfo);

        [JsonProperty("nodes")]
        public IEnumerable<GitHubRepository> RepositoryList { get; set; }

        [JsonProperty("pageInfo")]
        public PageInfo PageInfo { get; set; }
    }
}
