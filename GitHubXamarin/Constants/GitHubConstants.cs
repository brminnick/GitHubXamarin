using System.Collections.Generic;
using System;

namespace GitHubXamarin
{
    public static class GitHubConstants
    {
#error Missing Token, Follow these steps to create your Personal Access Token: https://help.github.com/articles/creating-a-personal-access-token-for-the-command-line/#creating-a-token
        public const string PersonalAccessToken = "Enter Token Here";
        public const string APIUrl = "https://api.github.com/graphql";
    }
}
