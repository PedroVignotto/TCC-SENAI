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
    public class ManutencoesViewModel : ViewModelBase
    {

        #region Command

        private DelegateCommand<Historico.Manutencao> _ExibirManutencaoCommand;
        public DelegateCommand<Historico.Manutencao> ExibirManutencaoCommand => _ExibirManutencaoCommand ?? (_ExibirManutencaoCommand = new DelegateCommand<Historico.Manutencao>(async (itemSelect) => await ExecuteExibirManutencaoCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variáveis

        private ObservableCollection<Historico.Manutencao> manutencoes;

        public ObservableCollection<Historico.Manutencao> Manutencoes
        {
            get => manutencoes;
            set => SetProperty(ref manutencoes, value);
        }

        private bool _isAuthorized;

        public bool IsAuthorized
        {
            get => _isAuthorized;
            set => SetProperty(ref _isAuthorized, value);
        }

        #endregion

        public ManutencoesViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Manuteções";
            Icone = "toolsIcon.png";
        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {
            UsuarioAtual = new Usuario();
            JsonUsuario = new JsonUsuario();
            Icon = new Icon();
            Manutencoes = new ObservableCollection<Historico.Manutencao>();

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            Body = false;
            Load = true;

            if (UsuarioAtual.Id_nivel_usuario == 1)
                IsAuthorized = true;
            else
                IsAuthorized = false;

            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                Manutencoes.Clear();

                var manutencaos = await HeritageAPIService.GetAsyncManutencoes(UsuarioAtual.Id_empresa);

                foreach (var manutencao in manutencaos)
                    Manutencoes.Add(manutencao);

                if (Manutencoes.Count == 0)
                    Null = true;
                else
                    Body = true;

                IsBusy = true;
            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar manutenções" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Null = true;

                throw ex;
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

        private async Task ExecuteExibirManutencaoCommand(Historico.Manutencao manutencao)
        {
            await NavigationService.NavigateAsync(new Uri("ExibirManutencao", UriKind.Relative));
        }

        public override void Destroy()
        {
            UsuarioAtual = null;
            JsonUsuario = null;
            Icon = null;
            Manutencoes = null;
        }

        #endregion

    }
}
