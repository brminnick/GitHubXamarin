using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace GitHubXamarin
{
    public class SettingsPage : BaseContentPage<SettingsViewModel>
    {
        const string _gitHubUserName = "GitHub User Name";
        const string _gitHubApiToken = "GitHub API Token";

        readonly Xamarin.Forms.Entry _tokenEntry;

        public SettingsPage()
        {
            ViewModel.SaveCompleted += HandleSaveCompleted;
            ViewModel.SaveFailed += HandleSaveFailed;

            var usernameLabel = new MediumBlueLabel(_gitHubUserName);

            var usernameEntry = new SettingsEntry(_gitHubUserName)
            {
                ReturnType = ReturnType.Next,
                ReturnCommand = new Command(() => _tokenEntry.Focus())
            };
            usernameEntry.SetBinding(Xamarin.Forms.Entry.TextProperty, nameof(ViewModel.UsernameEntryText));

            var tokenLabel = new MediumBlueLabel(_gitHubApiToken);

            _tokenEntry = new SettingsEntry(_gitHubApiToken)
            {
                IsPassword = true,
                ReturnType = ReturnType.Go
            };
            _tokenEntry.SetBinding(Xamarin.Forms.Entry.TextProperty, nameof(ViewModel.TokenEntryText));
            _tokenEntry.SetBinding(Xamarin.Forms.Entry.ReturnCommandProperty, nameof(ViewModel.SaveCommand));

            var saveButton = new BlueButton { Text = "Save" };
            saveButton.SetBinding(Button.CommandProperty, nameof(ViewModel.SaveCommand));

            var cancelButton = new BlueButton { Text = "Cancel" };
            cancelButton.Clicked += HandleCancelButtonClicked;

            On<iOS>().SetUseSafeArea(true);

            BackgroundColor = ColorConstants.LightBlue;

            Title = "Settings";

            Content = new StackLayout
            {
                Padding = new Thickness(20),
                Spacing = 1,

                Children =
                {
                    usernameLabel,
                    usernameEntry,
                    tokenLabel,
                    _tokenEntry,
                    saveButton,
                    cancelButton
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.GetTokenCommand?.Execute(null);
        }

        Task ClosePage() => Device.InvokeOnMainThreadAsync(() => Navigation.PopModalAsync());

        async void HandleCancelButtonClicked(object sender, EventArgs e) => await ClosePage();

        void HandleSaveCompleted(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Save Completed", "", "OK");
                await ClosePage();
            });
        }

        void HandleSaveFailed(object sender, string message) =>
            Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Save Failed", message, "OK"));

        class BlueButton : Button
        {
            public BlueButton()
            {
                BackgroundColor = ColorConstants.MediumBlue;
                TextColor = Color.White;
                Margin = new Thickness(0, 5);
            }
        }

        class MediumBlueLabel : Label
        {
            public MediumBlueLabel(string text)
            {
                Text = text;
                FontSize = 15;
                TextColor = ColorConstants.MediumBlue;
                Margin = new Thickness(0);
            }
        }

        class SettingsEntry : Xamarin.Forms.Entry
        {
            public SettingsEntry(string placeHolder)
            {
                Placeholder = placeHolder;
                PlaceholderColor = Color.Gray;
                Margin = new Thickness(0, 0, 0, 10);
            }
        }
    }
}
