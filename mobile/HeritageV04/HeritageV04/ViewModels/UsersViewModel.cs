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

namespace HeritageV04.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand RefreshCommand => new DelegateCommand(ExecuteRefreshCommand);

        private DelegateCommand<User> _DisplayUserCommand;
        public DelegateCommand<User> DisplayUserCommand => _DisplayUserCommand ?? (_DisplayUserCommand = new DelegateCommand<User>(async (itemSelect) => await ExecuteDisplayUserCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variables

        private ObservableCollection<User> users;

        public ObservableCollection<User> Users
        {
            get => users;
            set => SetProperty(ref users, value);
        }

        private bool _isAuthorized;

        public bool IsAuthorized
        {
            get => _isAuthorized;
            set => SetProperty(ref _isAuthorized, value);
        }

        #endregion

        public UsersViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {

            Title = "Usuários";
            Icone = "avatarIcon.png";

        }

        #region Methods

        public override async void Initialize(INavigationParameters parameters)
        {
            Users = new ObservableCollection<User>();
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

        private async Task ExecuteDisplayUserCommand(User user)
        {

            var navigationParams = new NavigationParameters
            {
                {"user", user}
            };

            if (user.UserLevel == 1)
                await NavigationService.NavigateAsync("DisplayAdmin", navigationParams);
            else if (user.UserLevel == 2)
                await NavigationService.NavigateAsync("DisplayManager", navigationParams);
            else if (user.UserLevel == 3)
                await NavigationService.NavigateAsync("DisplaySupport", navigationParams);

        }

        private async Task LoadAsync()
        {
            try
            {
                Body = false;
                Load = true;
                Null = false;

                ObservableCollection<User> users = await HeritageAPIService.GetAsyncUsers(CurrentUser.CompanyId);

                Users.Clear();

                foreach (User user in users)
                {
                    if (user.UserLevel == 1)
                        user.UserLevelDescription = "Administrador";
                    else if (user.UserLevel == 2)
                        user.UserLevelDescription = "Gerenciador";
                    else if (user.UserLevel == 3)
                        user.UserLevelDescription = "Suporte";

                    if (CurrentUser.Id != user.Id)
                        Users.Add(user);
                }

                if (Users.Count == 0)
                    Null = true;
                else
                    Body = true;

                IsBusy = true;
            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar usuários" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Null = true;

                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: Usuários");
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
