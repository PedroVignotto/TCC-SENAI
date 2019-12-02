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
    public class MovimentacoesViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand RefreshCommand { get; private set; }

        #endregion


        #region Variáveis

        private ObservableCollection<Historico.Movimentacao> movimentacoes;

        public ObservableCollection<Historico.Movimentacao> Movimentacoes
        {
            get { return movimentacoes; }
            set { SetProperty(ref movimentacoes, value); }
        }

        #endregion


        public MovimentacoesViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Movimentações";
            Icone = "accelerationIcon.png";

            RefreshCommand = new DelegateCommand(ExecuteRefreshCommand);
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            Movimentacoes = new ObservableCollection<Historico.Movimentacao>();
            UsuarioAtual = new Usuario();
            JsonUsuario = new JsonUsuario();
            Icon = new Icon();

            Body = true;

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            await LoadAsync();
        }

        private async void ExecuteRefreshCommand()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                IsBusy = true;
                Body = false;
                Load = true;

                var historicos = await HeritageAPIService.GetAsyncMovimentacoes(UsuarioAtual.Id_empresa);

                foreach (var historico in historicos)
                    Movimentacoes.Add(historico);

                if (Movimentacoes.Count > 0)
                    Body = true;
                else
                    Null = true;

            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar movimentações" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                Null = true;

                throw ex;
            }
            finally
            {
                Load = false;
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

    }
}
