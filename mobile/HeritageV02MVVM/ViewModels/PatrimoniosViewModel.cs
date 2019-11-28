using HeritageV02MVVM.Models;
using HeritageV02MVVM.Services.Abstraction;
using HeritageV02MVVM.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeritageV02MVVM.ViewModels
{
    public class PatrimoniosViewModel : ViewModelBase
    {

        #region Variáveis

        private ObservableCollection<Patrimonio> patrimonios;

        public ObservableCollection<Patrimonio> Patrimonios
        {
            get => patrimonios;
            set => SetProperty(ref patrimonios, value); 
        }

        private bool _isAuthorized;

        public bool IsAuthorized
        {
            get => _isAuthorized;
            set => SetProperty(ref _isAuthorized, value); 
        }

        #endregion

        #region Command

        private DelegateCommand<Patrimonio> _ExibirPatrimonioCommand;
        public DelegateCommand<Patrimonio> ExibirPatrimonioCommand => _ExibirPatrimonioCommand ?? (_ExibirPatrimonioCommand = new DelegateCommand<Patrimonio>(async (itemSelect) => await ExecuteExibirPatrimonioCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        public PatrimoniosViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Patrimônios";
            Icone = "boxIcon.png";
        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {
            Patrimonios = new ObservableCollection<Patrimonio>();
            UsuarioAtual = new Usuario();
            JsonUsuario = new JsonUsuario();
            Icon = new Icon();

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            if (UsuarioAtual.Id_nivel_usuario == 1)
                IsAuthorized = true;
            else
                IsAuthorized = false;

            await LoadAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadAsync();
        }

        private async Task ExecuteExibirPatrimonioCommand(Patrimonio patrimonio)
        {
            var navigationParams = new NavigationParameters
            {
                {"patrimonio", patrimonio}
            };

            await NavigationService.NavigateAsync("ExibirPatrimonio", navigationParams);
        }

        private async Task LoadAsync()
        {

            Load = true;

            try
            {
                ObservableCollection<Patrimonio> patrimonios = await HeritageAPIService.GetAsyncPatrimonios(UsuarioAtual.Id_empresa);

                Patrimonios.Clear();

                foreach (Patrimonio patrimonio in patrimonios)
                {
                    Patrimonios.Add(patrimonio);
                }

                if (Patrimonios.Count == 0)
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
                    { "Icon", Icon.IconName("bug") }
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

        public override void Destroy()
        {
            Patrimonios = null;
            UsuarioAtual = null;
            JsonUsuario = null;
            Icon = null;
        }

        #endregion

    }
}
