using HeritageV04.Models;
using HeritageV04.Services.Abstractions;
using HeritageV04.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Environment = HeritageV04.Models.Environment;

namespace HeritageV04.ViewModels
{
    public class EnvironmentsViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand RefreshCommand => new DelegateCommand(ExecuteRefreshCommand);

        private DelegateCommand<Environment> _DisplayEnvironmentCommand;
        public DelegateCommand<Environment> DisplayEnvironmentCommand => _DisplayEnvironmentCommand ?? (_DisplayEnvironmentCommand = new DelegateCommand<Environment>(async (itemSelect) => await ExecuteDisplayEnvironmentCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variables

        private ObservableCollection<Environment> environments;

        public ObservableCollection<Environment> Environments
        {
            get => environments;
            set => SetProperty(ref environments, value);
        }

        private bool _isAuthorized;

        public bool IsAuthorized
        {
            get => _isAuthorized;
            set => SetProperty(ref _isAuthorized, value);
        }

        #endregion

        public EnvironmentsViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Ambientes";
            Icone = "placeholderIcon.png";
        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {
            Environments = new ObservableCollection<Environment>();
            UserJson = new UserJson();
            CurrentUser = new User();
            IconTheme = new IconTheme();

            CurrentUser = UserJson.GetUsuarioJson();

            if (CurrentUser.UserLevel == 1)
                IsAuthorized = true;
            else
                IsAuthorized = false;

            await LoadAsync();

        }

        private async void ExecuteRefreshCommand()
        {
            await LoadAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadAsync();
        }

        private async Task ExecuteDisplayEnvironmentCommand(Environment environment)
        {
            var navigationParams = new NavigationParameters
            {
                {"environment", environment}
            };

            await NavigationService.NavigateAsync("DisplayEnvironment", navigationParams);
        }

        private async Task LoadAsync()
        {
            try
            {
                Body = false;
                Load = true;
                Null = false;

                ObservableCollection<Environment> environments = await HeritageAPIService.GetAsyncEnvironments(CurrentUser.CompanyId);
                ObservableCollection<Heritage> heritages = await HeritageAPIService.GetAsyncHeritages(CurrentUser.CompanyId);

                Environments.Clear();

                foreach (Environment environment in environments)
                {
                    foreach (Heritage heritage in heritages)
                    {
                        if (heritage.EnvironmentId == environment.Id)
                            environment.QuantityHeritages++;
                    }

                    Environments.Add(environment);
                }

                if (Environments.Count == 0)
                    Null = true;
                else
                    Body = true;

                IsBusy = true;
            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar ambientes" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Null = true;

                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: Ambientes");
            }
            finally
            {
                IsBusy = false;
                Load = false;
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

        #endregion

    }
}
