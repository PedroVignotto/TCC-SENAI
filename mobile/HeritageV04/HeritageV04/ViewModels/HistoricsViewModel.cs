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
    public class HistoricsViewModel : ViewModelBase
    {

        public DelegateCommand RefreshCommand => new DelegateCommand(ExecuteRefreshCommand);

        private ObservableCollection<Historic> historics;
        public ObservableCollection<Historic> Historics
        {
            get { return historics; }
            set { SetProperty(ref historics, value); }
        }

        public HistoricsViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Historicos";
        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {
            Historics = new ObservableCollection<Historic>();
            UserJson = new UserJson();
            CurrentUser = new User();
            IconTheme = new IconTheme();

            CurrentUser = UserJson.GetUsuarioJson();

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

        private async Task LoadAsync()
        {
            try
            {
                Body = false;
                Load = true;
                Null = false;

                ObservableCollection<Historic> historics = await HeritageAPIService.GetAsyncHistorics(CurrentUser.CompanyId);

                Historics.Clear();

                foreach (Historic historic in historics)
                    Historics.Add(historic);

                if (Historics.Count == 0)
                    Null = true;
                else
                    Body = true;

                IsBusy = true;
            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar os historicos" },
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
