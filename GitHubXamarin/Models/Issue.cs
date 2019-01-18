using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class Issues
    {
        [JsonProperty("nodes")]
        public List<Issue> IssueList { get; set; }
    }

    public class Issue
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("closedAt")]
        public DateTimeOffset? ClosedAt { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}
