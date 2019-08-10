using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;

using Refit;
using Xamarin.Forms;

namespace GitHubXamarin
{
    public class RepositoryViewModel : BaseViewModel
    {
        readonly WeakEventManager<PullToRefreshFailedEventArgs> _pullToRefreshFailedEventManager = new WeakEventManager<PullToRefreshFailedEventArgs>();

        bool _isRefreshing;
        ICommand _pullToRefreshCommand, _filterRepositoriesCommand;
        string _searchBarText = "";
        IReadOnlyList<Repository> _repositoryList;

        public event EventHandler<PullToRefreshFailedEventArgs> PullToRefreshFailed
        {
            add => _pullToRefreshFailedEventManager.AddEventHandler(value);
            remove => _pullToRefreshFailedEventManager.RemoveEventHandler(value);
        }

        public ICommand PullToRefreshCommand => _pullToRefreshCommand ??
            (_pullToRefreshCommand = new AsyncCommand(() => ExecutePullToRefreshCommand(GitHubSettings.User)));

        public ICommand FilterRepositoriesCommand => _filterRepositoriesCommand ??
            (_filterRepositoriesCommand = new Command<string>(text => SetSearchBarText(text)));

        public ObservableRangeCollection<Repository> VisibleRepositoryCollection { get; } = new ObservableRangeCollection<Repository>();

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        void SetRepositoriesCollection(in IEnumerable<Repository> repositories, string repositoryOwner, string searchBarText)
        {
            _repositoryList = repositories.Where(x => x.Owner.Login.Equals(repositoryOwner, StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(x => x.StarCount).ToList();

            IEnumerable<Repository> filteredRepositoryList;
            if (string.IsNullOrWhiteSpace(searchBarText))
                filteredRepositoryList = _repositoryList;
            else
                filteredRepositoryList = _repositoryList.Where(x => x.Name.Contains(searchBarText));

            VisibleRepositoryCollection.Clear();
            VisibleRepositoryCollection.AddRange(filteredRepositoryList);
        }

        async Task ExecutePullToRefreshCommand(string repositoryOwner)
        {
            try
            {
                var repositoryList = await GitHubGraphQLService.GetRepositories(repositoryOwner).ConfigureAwait(false);

                SetRepositoriesCollection(repositoryList, repositoryOwner, _searchBarText);
            }
            catch (ApiException e) when (e.StatusCode is System.Net.HttpStatusCode.Unauthorized)
            {
                OnPullToRefreshFailed("Invalid Api Token", "Edit the Github API Token in Settings");
            }
            catch (Exception e)
            {
                OnPullToRefreshFailed("Error", e.Message);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        void SetSearchBarText(in string text)
        {
            if (EqualityComparer<string>.Default.Equals(_searchBarText, text))
                return;

            _searchBarText = text;

            if (_repositoryList?.Any() is true)
                SetRepositoriesCollection(_repositoryList, GitHubSettings.User, text);
        }

        void OnPullToRefreshFailed(string title, string message) =>
            _pullToRefreshFailedEventManager.HandleEvent(this, new PullToRefreshFailedEventArgs(message, title), nameof(PullToRefreshFailed));
    }
}
