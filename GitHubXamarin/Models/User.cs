﻿using System;
using System.Text;
using Newtonsoft.Json;

namespace GitHubXamarin
{
    public class User
    {
        public User() { }

        [JsonConstructor]
        public User(GitHubFollowers followers) => Followers = followers;

        [JsonProperty("repositories")]
        public RepositoryConnection RepositoryConnection { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset AccountCreationDate { get; set; }

        public int FollowerCount
        {
            get => Followers?.Count ?? -1;
            set
            {
                if (Followers is null)
                    Followers = new GitHubFollowers { Count = value };
                else
                    Followers.Count = value;
            }
        }

        [JsonProperty("followers")]
        GitHubFollowers Followers { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{nameof(Name)}: {Name}");
            stringBuilder.AppendLine($"{nameof(Company)}: {Company}");
            stringBuilder.AppendLine($"{nameof(FollowerCount)}: {FollowerCount}");
            stringBuilder.AppendLine($"{nameof(AccountCreationDate)}: {AccountCreationDate}");

            return stringBuilder.ToString();
        }
    }

    public class GitHubFollowers
    {
        [JsonProperty("totalCount")]
        public int Count { get; set; }
    }
}
