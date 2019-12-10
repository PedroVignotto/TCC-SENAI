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
    public class HomeViewModel : ViewModelBase
    {

        #region Variables

        private int _numberHeritages;

        public int NumberHeritages
        {
            get => _numberHeritages;
            set => SetProperty(ref _numberHeritages, value);
        }

        private int _numberEnvironments;

        public int NumberEnvironments
        {
            get => _numberEnvironments;
            set => SetProperty(ref _numberEnvironments, value);
        }

        private int _numberUsers;

        public int NumberUsers
        {
            get => _numberUsers;
            set => SetProperty(ref _numberUsers, value);
        }

        private int _numberManagers;

        public int NumberManagers
        {
            get => _numberManagers;
            set => SetProperty(ref _numberManagers, value);
        }

        private int _numberAdmins;

        public int NumberAdmins
        {
            get => _numberAdmins;
            set => SetProperty(ref _numberAdmins, value);
        }

        private int _numberSupports;

        public int NumberSupports
        {
            get => _numberSupports;
            set => SetProperty(ref _numberSupports, value);
        }

        #endregion

        public HomeViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Home";
            Icone = "homeIcon.png";
        }

        #region Methods

        public override async void Initialize(INavigationParameters parameters)
        {
            CurrentUser = new User();
            UserJson = new UserJson();
            IconTheme = new IconTheme();
            CurrentUser = UserJson.GetUsuarioJson();

            await LoadAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                ObservableCollection<Heritage> heritages = await HeritageAPIService.GetAsyncHeritages(CurrentUser.CompanyId);
                ObservableCollection<Heritage> heritages_publish = new ObservableCollection<Heritage>();

                NumberHeritages = 0;

                foreach (Heritage heritage in heritages)
                {
                    NumberHeritages++;

                    if (NumberHeritages >= heritages.Count - 3)
                        heritages_publish.Add(heritage);
                }

                ObservableCollection<Environment> environments = await HeritageAPIService.GetAsyncEnvironments(CurrentUser.CompanyId);

                NumberEnvironments = 0;

                foreach (Environment environment in environments)
                    NumberEnvironments++;

                ObservableCollection<User> users = await HeritageAPIService.GetAsyncUsers(CurrentUser.CompanyId);

                NumberUsers = 0;
                NumberManagers = 0;
                NumberAdmins = 0;
                NumberSupports = 0;

                foreach (User user in users)
                {
                    NumberUsers++;

                    if (user.UserLevel == 2)
                        NumberManagers++;
                    else if (user.UserLevel == 3)
                        NumberSupports++;
                    else if (user.UserLevel == 1)
                        NumberAdmins++;
                }

                IsBusy = true;
            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar algumas informções" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                throw ex;
            }
            finally
            {
                IsBusy = false;
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

        #endregion

    }
}
