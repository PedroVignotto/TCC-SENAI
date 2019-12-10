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
    public class HeritagesViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand RefreshCommand => new DelegateCommand(ExecuteRefreshCommand);

        private DelegateCommand<Heritage> _DisplayHeritageCommand;
        public DelegateCommand<Heritage> DisplayHeritageCommand => _DisplayHeritageCommand ?? (_DisplayHeritageCommand = new DelegateCommand<Heritage>(async (itemSelect) => await ExecuteDisplayHeritageCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variables

        private ObservableCollection<Heritage> heritages;

        public ObservableCollection<Heritage> Heritages
        {
            get => heritages;
            set => SetProperty(ref heritages, value);
        }

        private bool _isAuthorized;

        public bool IsAuthorized
        {
            get => _isAuthorized;
            set => SetProperty(ref _isAuthorized, value);
        }

        #endregion

        public HeritagesViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Patrimônios";
            Icone = "boxIcon.png";
        }

        #region Methods

        public override async void Initialize(INavigationParameters parameters)
        {
            Heritages = new ObservableCollection<Heritage>();
            CurrentUser = new User();
            UserJson = new UserJson();
            IconTheme = new IconTheme();

            CurrentUser = UserJson.GetUsuarioJson();

            if (CurrentUser.UserLevel == 1)
                IsAuthorized = true;
            else
                IsAuthorized = false;

            await LoadAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadAsync();
        }

        private async Task ExecuteDisplayHeritageCommand(Heritage heritage)
        {
            var navigationParams = new NavigationParameters
            {
                {"heritage", heritage}
            };

            await NavigationService.NavigateAsync("DisplayHeritage", navigationParams);
        }

        private async void ExecuteRefreshCommand()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            Body = false;
            Load = true;
            Null = false;

            try
            {
                ObservableCollection<Heritage> heritages = await HeritageAPIService.GetAsyncHeritages(CurrentUser.CompanyId);

                Heritages.Clear();

                foreach (Heritage heritage in heritages)
                    Heritages.Add(heritage);

                if (Heritages.Count == 0)
                    Null = true;
                else
                    Body = true;

                IsBusy = true;
            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar patrimônios" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Null = true;
                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: Patrimônios");
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
