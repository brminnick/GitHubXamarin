﻿using Newtonsoft.Json;

namespace GitHubXamarin
{
    abstract class GraphQLRequest
    {
        public GraphQLRequest(string query, string variables = null)
        {
            Query = query;
            Variables = variables;
        }

        [JsonProperty("query")]
        public string Query { get; }

        [JsonProperty("variables")]
        public string Variables { get; }
    }
}
