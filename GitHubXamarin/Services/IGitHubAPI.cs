using System.Threading.Tasks;
using Refit;

namespace GitHubXamarin
{
    [Headers("Content-Type: application/json",
                "Authorization: bearer " + GitHubConstants.PersonalAccessToken,
                "User-Agent: GitHubXamarin")]
    interface IGitHubAPI
    {
        [Post("")]
        Task<GraphQLResponse<GitHubUserResponse>> UserQuery([Body] GraphQLRequest request);

        [Post("")]
        Task<GraphQLResponse<RepositoryResponse>> RepositoryQuery([Body] GraphQLRequest request);

        [Post("")]
        Task<GraphQLResponse<RepositoryConnectionResponse>> RepositoryConnectionQuery([Body] GraphQLRequest request);
    }
}
