using System.Collections.Generic;
using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class IssuesConnection
    {
        [JsonProperty("nodes")]
        public List<Issue> IssueList { get; set; }

        [JsonProperty("totalCount")]
        public int IssuesCount { get; set; }
    }
}
