using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;

namespace GitHubXamarin
{
    class GitHubRepositoryViewModel : BaseViewModel
    {
        bool _isRefreshing;
        ICommand _pullToRefreshCommand;
        ObservableCollection<Repository> _repositoryCollection = new ObservableCollection<Repository>();

        public ICommand PullToRefreshCommand => _pullToRefreshCommand ??
            (_pullToRefreshCommand = new AsyncCommand(() => ExecutePullToRefreshCommand("brminnick"), continueOnCapturedContext: false));

        public ObservableCollection<Repository> RepositoryCollection
        {
            get => _repositoryCollection;
            set => SetProperty(ref _repositoryCollection, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        async Task ExecutePullToRefreshCommand(string repositoryOwner)
        {
            try
            {
                var repositoryList = await GitHubGraphQLService.GetRepositories(repositoryOwner).ConfigureAwait(false); 

                foreach (var repository in repositoryList.OrderByDescending(x => x.StarCount))
                {
                    _repositoryCollection.Add(repository);
                }
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
