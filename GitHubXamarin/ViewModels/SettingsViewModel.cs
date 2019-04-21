using System;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;

namespace GitHubXamarin
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Constant Fields
        readonly WeakEventManager _saveCompletedEventManager = new WeakEventManager();
        readonly WeakEventManager<string> _saveFailedEventManager = new WeakEventManager<string>();
        #endregion

        #region Fields
        ICommand _getTokenCommand, _saveCommand;
        string _tokenEntryText;
        string _usernameEntryText = GitHubSettings.User;
        #endregion

        #region Events
        public event EventHandler SaveCompleted
        {
            add => _saveCompletedEventManager.AddEventHandler(value);
            remove => _saveCompletedEventManager.RemoveEventHandler(value);
        }

        public event EventHandler<string> SaveFailed
        {
            add => _saveFailedEventManager.AddEventHandler(value);
            remove => _saveFailedEventManager.RemoveEventHandler(value);
        }
        #endregion

        #region Properties
        public ICommand GetTokenCommand => _getTokenCommand ??
            (_getTokenCommand = new AsyncCommand(async () => TokenEntryText = await GitHubSettings.GetToken().ConfigureAwait(false), continueOnCapturedContext: false));

        public ICommand SaveCommand => _saveCommand ??
            (_saveCommand = new AsyncCommand(() => ExecuteSaveButtonTapped(UsernameEntryText, TokenEntryText), continueOnCapturedContext: false));

        public string UsernameEntryText
        {
            get => _usernameEntryText;
            set => SetProperty(ref _usernameEntryText, value);
        }

        public string TokenEntryText
        {
            get => _tokenEntryText;
            set => SetProperty(ref _tokenEntryText, value);
        }
        #endregion

        #region Methods
        async Task ExecuteSaveButtonTapped(string usernameEntryText, string tokenEntryText)
        {
            try
            {
                GitHubSettings.User = usernameEntryText;
                await GitHubSettings.SetToken(tokenEntryText).ConfigureAwait(false);

                OnSaveCompleted();
            }
            catch (Exception e)
            {
                OnSaveFailed(e.Message);
            }
        }

        void OnSaveCompleted() => _saveCompletedEventManager.HandleEvent(this, EventArgs.Empty, nameof(SaveCompleted));
        void OnSaveFailed(string message) => _saveFailedEventManager.HandleEvent(this, message, nameof(SaveFailed));
        #endregion
    }
}
