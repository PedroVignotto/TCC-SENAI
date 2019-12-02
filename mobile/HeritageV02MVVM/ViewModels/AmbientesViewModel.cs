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
    public class AmbientesViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand RefreshCommand { get; set; }

        private DelegateCommand<Ambiente> _ExibirAmbienteCommand;
        public DelegateCommand<Ambiente> ExibirAmbienteCommand => _ExibirAmbienteCommand ?? (_ExibirAmbienteCommand = new DelegateCommand<Ambiente>(async (itemSelect) => await ExecuteExibirAmbienteCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variáveis

        private ObservableCollection<Ambiente> ambientes;

        public ObservableCollection<Ambiente> Ambientes
        {
            get => ambientes;
            set => SetProperty(ref ambientes, value);
        }

        private bool _isAuthorized;

        public bool IsAuthorized
        {
            get => _isAuthorized; 
            set => SetProperty(ref _isAuthorized, value); 
        }

        #endregion

        public AmbientesViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Ambientes";
            Icone = "placeholderIcon.png";

            RefreshCommand = new DelegateCommand(ExecuteRefreshCommand);
        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {
            Ambientes = new ObservableCollection<Ambiente>();
            JsonUsuario = new JsonUsuario();
            UsuarioAtual = new Usuario();
            Icon = new Icon();

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            if (UsuarioAtual.Id_nivel_usuario == 1)
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

        private async Task ExecuteExibirAmbienteCommand(Ambiente ambiente)
        {
            var navigationParams = new NavigationParameters
            {
                {"ambiente", ambiente}
            };

            await NavigationService.NavigateAsync("ExibirAmbiente", navigationParams);
        }

        private async Task LoadAsync()
        {
            try
            {
                Body = false;
                Load = true;
                Null = false;

                ObservableCollection<Ambiente> ambientes = await HeritageAPIService.GetAsyncAmbientes(UsuarioAtual.Id_empresa);
                ObservableCollection<Patrimonio> patrimonios = await HeritageAPIService.GetAsyncPatrimonios(UsuarioAtual.Id_empresa);

                Ambientes.Clear();

                foreach (Ambiente ambiente in ambientes)
                {
                    foreach (Patrimonio patrimonio in patrimonios)
                    {
                        if (patrimonio.Id_ambiente == ambiente.Id)
                            ambiente.Quantidade_patrimonio++;
                    }

                    Ambientes.Add(ambiente);
                }

                if (Ambientes.Count == 0)
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
                    { "Icon", Icon.IconName("bug") }
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

        public override void Destroy()
        {
            Ambientes = null;
            JsonUsuario = null;
            UsuarioAtual = null;
            Icon = null;
        }

        #endregion

    }
}
