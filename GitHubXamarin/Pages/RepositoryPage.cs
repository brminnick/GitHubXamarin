using System;
using System.Linq;
using AsyncAwaitBestPractices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GitHubXamarin
{
    public class RepositoryPage : BaseContentPage<RepositoryViewModel>, ISearchPage
    {
        readonly WeakEventManager<string> _searchBarTextChangedEventManager = new WeakEventManager<string>();
        readonly ListView _listView;

        public RepositoryPage()
        {
            SearchBarTextChanged += HandleSearchBarTextChanged;
            ViewModel.PullToRefreshFailed += HandlePullToRefreshFailed;

            _listView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                IsPullToRefreshEnabled = true,
                ItemTemplate = new DataTemplate(typeof(RepositoryViewCell)),
                SeparatorVisibility = SeparatorVisibility.None,
                RowHeight = RepositoryViewCell.ImageHeight,
                RefreshControlColor = Device.RuntimePlatform is Device.iOS ? Color.White : ColorConstants.DarkBlue,
                BackgroundColor = ColorConstants.LightBlue,
                SelectionMode = ListViewSelectionMode.None
            };
            _listView.ItemTapped += HandleListViewItemTapped;
            _listView.SetBinding(ListView.IsRefreshingProperty, nameof(ViewModel.IsRefreshing));
            _listView.SetBinding(ListView.ItemsSourceProperty, nameof(ViewModel.VisibleRepositoryCollection));
            _listView.SetBinding(ListView.RefreshCommandProperty, nameof(ViewModel.PullToRefreshCommand));

            var settingsToolbarItem = new ToolbarItem
            {
                Order = Device.RuntimePlatform is Device.Android ? ToolbarItemOrder.Secondary : ToolbarItemOrder.Default
            };

            if (Device.RuntimePlatform is Device.iOS)
                settingsToolbarItem.IconImageSource = "Settings";
            else
                settingsToolbarItem.Text = "Settings";

            settingsToolbarItem.Clicked += HandleSettingsToolbarItem;
            ToolbarItems.Add(settingsToolbarItem);

            Title = "Repositories";
            BackgroundColor = ColorConstants.LightBlue;

            Content = _listView;
        }

        public event EventHandler<string> SearchBarTextChanged
        {
            add => _searchBarTextChangedEventManager.AddEventHandler(value);
            remove => _searchBarTextChangedEventManager.RemoveEventHandler(value);
        }

        public void OnSearchBarTextChanged(in string text) => _searchBarTextChangedEventManager.HandleEvent(this, text, nameof(SearchBarTextChanged));

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _listView.BeginRefresh();
        }

        async void HandleListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView listView)
                listView.SelectedItem = null;

            if (e.Item is Repository repository && repository.Uri.IsAbsoluteUri && repository.Uri.Scheme.Equals(Uri.UriSchemeHttps))
            {
                var browserOptions = new BrowserLaunchOptions
                {
                    PreferredToolbarColor = ColorConstants.LightBlue,
                    PreferredControlColor = Color.DarkBlue
                };

                await Browser.OpenAsync(repository.Uri, browserOptions);
            }
        }

        void HandleSettingsToolbarItem(object sender, EventArgs e) =>
            Device.BeginInvokeOnMainThread(async () => await Navigation.PushModalAsync(new BaseNavigationPage(new SettingsPage())));

        void HandlePullToRefreshFailed(object sender, PullToRefreshFailedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!Application.Current.MainPage.Navigation.ModalStack.Any()
                    && Application.Current.MainPage.Navigation.NavigationStack.Last() is RepositoryPage)
                {
                    await DisplayAlert(e.ErrorTitle, e.ErrorMessage, "OK");
                }
            });
        }

        void HandleSearchBarTextChanged(object sender, string searchBarText) => ViewModel.FilterRepositoriesCommand?.Execute(searchBarText);
    }
}
