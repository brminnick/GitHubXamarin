using System;
using Newtonsoft.Json;

namespace GitHubXamarin
{
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
