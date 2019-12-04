using HeritageV02MVVM.Models;
using HeritageV02MVVM.Services.Abstraction;
using HeritageV02MVVM.Utilities;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeritageV02MVVM.ViewModels
{
    public class ExibirPatrimonioViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand SalvarCommand { get; private set; }

        public DelegateCommand ExcluirCommand { get; private set; }

        #endregion

        #region Variáveis

        private Patrimonio patrimonio;

        [AutoInitialize(true)]
        public Patrimonio Patrimonio
        {
            get { return patrimonio; }
            set { SetProperty(ref patrimonio, value); }
        }

        private ObservableCollection<Ambiente> ambientes;
        public ObservableCollection<Ambiente> Ambientes
        {
            get { return ambientes; }
            set { SetProperty(ref ambientes, value); }
        }

        private Ambiente ambiente;
        public Ambiente Ambiente
        {
            get { return ambiente; }
            set { SetProperty(ref ambiente, value); }
        }

        private ObservableCollection<Historico.Movimentacao> movimentacaos;
        public ObservableCollection<Historico.Movimentacao> Movimentacoes
        {
            get { return movimentacaos; }
            set { SetProperty(ref movimentacaos, value); }
        }

        private ObservableCollection<Historico.Manutencao> manutencos;
        public ObservableCollection<Historico.Manutencao> Manutencoes
        {
            get { return manutencos; }
            set { SetProperty(ref manutencos, value); }
        }

        private string _iconNome;
        public string IconNome
        {
            get { return _iconNome; }
            set { SetProperty(ref _iconNome, value); }
        }

        private string _iconCodigo;
        public string IconCodigo
        {
            get { return _iconCodigo; }
            set { SetProperty(ref _iconCodigo, value); }
        }

        private string _iconAmbiente;
        public string IconAmbiente
        {
            get { return _iconAmbiente; }
            set { SetProperty(ref _iconAmbiente, value); }
        }

        private string _iconDescricao;
        public string IconDescricao
        {
            get => _iconDescricao;
            set { SetProperty(ref _iconDescricao, value); }
        }

        private string _iconExcluir;
        public string IconExcluir
        {
            get => _iconExcluir;
            set { SetProperty(ref _iconExcluir, value); }
        }
        

        private string _loadMessage;
        public string LoadMessage
        {
            get => _loadMessage;
            set { SetProperty(ref _loadMessage, value); }
        }

        private bool _bodyMovimentacoes;

        public bool BodyMovimentacoes
        {
            get => _bodyMovimentacoes;
            set => SetProperty(ref _bodyMovimentacoes, value);
        }

        private bool _loadMovimentacoes;

        public bool LoadMovimentacoes
        {
            get => _loadMovimentacoes;
            set => SetProperty(ref _loadMovimentacoes, value);
        }

        private bool _nullMovimentacoes;

        public bool NullMovimentacoes
        {
            get => _nullMovimentacoes;
            set => SetProperty(ref _nullMovimentacoes, value);
        }

        private bool _bodyManutencoes;

        public bool BodyManutencoes
        {
            get => _bodyManutencoes;
            set => SetProperty(ref _bodyManutencoes, value);
        }

        private bool _loadManutencoes;

        public bool LoadManutencoes
        {
            get => _loadManutencoes;
            set => SetProperty(ref _loadManutencoes, value);
        }

        private bool _nullManutencoes;

        public bool NullManutencoes
        {
            get => _nullManutencoes;
            set => SetProperty(ref _nullManutencoes, value);
        }

        private int _heightListViewMovimentacoes;

        public int HeightListViewMovimentacoes
        {
            get { return _heightListViewMovimentacoes; }
            set => SetProperty(ref _heightListViewMovimentacoes, value);
        }

        private int _heightListViewManutencoes;

        public int HeightListViewManutencoes
        {
            get { return _heightListViewManutencoes; }
            set => SetProperty(ref _heightListViewManutencoes, value);
        } 

        #endregion

        public ExibirPatrimonioViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Exibir patrimônio";

            SalvarCommand = new DelegateCommand(ExecuteSalvarCommand);
            ExcluirCommand = new DelegateCommand(ExecuteExcluirCommand);
        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {

            Patrimonio = new Patrimonio();
            Ambiente = new Ambiente();
            Ambientes = new ObservableCollection<Ambiente>();
            Manutencoes = new ObservableCollection<Historico.Manutencao>();
            Movimentacoes = new ObservableCollection<Historico.Movimentacao>();
            UsuarioAtual = new Usuario();
            JsonUsuario = new JsonUsuario();
            Icon = new Icon();

            if (parameters.ContainsKey("patrimonio"))
                Patrimonio = (Patrimonio)parameters["patrimonio"];

            Icone = Icon.IconName("box");
            IconAmbiente = Icon.IconName("placeholder");
            IconCodigo = Icon.IconName("qrcode");
            IconDescricao = Icon.IconName("document");
            IconExcluir = Icon.IconName("cancel");
            IconNome = Icon.IconName("name");

            if (Patrimonio.Estado_patrimonio == true)
                Patrimonio.Estado_mensagem = "Patrimônio verificado";
            else
                Patrimonio.Estado_mensagem = "Patrimônio não verificado";

            Body = true;

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            await LoadAsync();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var navigationMode = parameters.GetNavigationMode();
            if (navigationMode == NavigationMode.Back)
            {
                string codigo = null;

                if (parameters.ContainsKey("codigo"))
                    codigo = (string)parameters["codigo"];

                if (Patrimonio.Codigo_patrimonio == codigo)
                {
                    Patrimonio.Estado_patrimonio = true;
                    Patrimonio.Estado_mensagem = "Patrimônio verificado";
                }

                if (parameters.ContainsKey("codigo"))
                    Patrimonio.Codigo_patrimonio = (string)parameters["codigo"];

            }
        }

        private async Task LoadAsync()
        {

            BodyManutencoes = false;
            BodyMovimentacoes = false;

            LoadManutencoes = true;
            LoadMovimentacoes = true;

            try
            {
                ObservableCollection<Ambiente> ambientes = await HeritageAPIService.GetAsyncAmbientes(UsuarioAtual.Id_empresa);

                foreach (Ambiente ambiente in ambientes)
                {
                    if (ambiente.Id == Patrimonio.Id_ambiente)
                        Ambiente = ambiente;
                    Ambientes.Add(ambiente);
                }
                    

                if (Ambientes.Count < 0)
                    Ambientes = new ObservableCollection<Ambiente>() { new Ambiente() { Nome_ambiente = "Sem ambientes para exibir" } };
            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar ambiente" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                throw ex;
            }
            finally
            {
                IsBusy = false;
            }

            try
            {
                //ObservableCollection<Historico.Movimentacao> movimentacoes = await HeritageAPIService.GetAsyncMovimentacoesPatrimonio(UsuarioAtual.Id_empresa, Patrimonio.Id);

                //foreach (Historico.Movimentacao movimentacao in movimentacoes)
                //    Movimentacoes.Add(movimentacao);

                //if (Movimentacoes.Count > 0)
                //    BodyMovimentacoes = true;
                //else
                //    NullMovimentacoes = true;

                //HeightListViewMovimentacoes = Movimentacoes.Count * 145;

                //IsBusy = true;
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

                NullMovimentacoes = true;

                throw ex;
            }
            finally
            {
                IsBusy = false;
                LoadMovimentacoes = false;
            }

            try
            {
                //ObservableCollection<Historico.Manutencao> manutencoes = await HeritageAPIService.GetAsyncManutencoesPatrimonio(UsuarioAtual.Id_empresa, Patrimonio.Id);

                //foreach (Historico.Manutencao manutencao in manutencoes)
                //    Manutencoes.Add(manutencao);

                //if (Manutencoes.Count > 0)
                //{
                //    HeightListViewManutencoes = Manutencoes.Count * 145;
                //    BodyMovimentacoes = true;
                //}
                //else
                //    NullMovimentacoes = true;

                //IsBusy = true;
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

                NullMovimentacoes = true;

                throw ex;
            }
            finally
            {
                IsBusy = false;
                LoadMovimentacoes = false;
            }
        }

        private async void ExecuteSalvarCommand()
        {
            try
            {
                if (UsuarioAtual.Id_nivel_usuario == 1)
                {
                    if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja salvar as alterações?", "Sim", "Não"))
                    {
                        Load = true;
                        Body = false;
                        LoadMessage = "Atualizando patrimônio";

                        if (Ambiente.Id != Patrimonio.Id_ambiente)
                        {
                            Historico historico = new Historico()
                            {
                                Nome_historico = 1,
                                Id_ambiente = Patrimonio.Id_ambiente,
                                Id_empresa = Patrimonio.Id_empresa,
                                Local_destino = Ambiente.Nome_ambiente,
                                Id_patrimonio = Patrimonio.Id,
                                Descricao = "Patrimônio " + Patrimonio.Codigo_patrimonio + " movimentado para " + Ambiente.Nome_ambiente,
                            };

                            bool insert = await HeritageAPIService.SetAsync(historico);

                            if (insert)
                            {
                                Patrimonio.Id_ambiente = Ambiente.Id;
                                bool up = await HeritageAPIService.PutAsync(Patrimonio);

                                if (up)
                                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Patrimônio alterado com sucesso");
                                else
                                    await PageDialogService.DisplayAlertAsync("Erro", "Erro ao alterar patrimônio", "Ok");
                            }
                            else
                                await PageDialogService.DisplayAlertAsync("Erro", "Erro ao fazer movimentação. Tente novamente", "Ok");
                        }
                        else
                        {
                            Patrimonio.Id_ambiente = Ambiente.Id;
                            bool up = await HeritageAPIService.PutAsync(Patrimonio);

                            if (up)
                                Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Patrimônio alterado com sucesso");
                            else
                                await PageDialogService.DisplayAlertAsync("Erro", "Erro ao alterar patrimônio", "Ok");
                        }
                    }
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a alteração");
            }
            catch (Exception ex)
            {
                Body = true;
                await PageDialogService.DisplayAlertAsync("Erro", "Erro ao alterar patrimônio", "Ok");

                throw ex;
            }
            finally
            {
                Load = false;
                Body = true;
            }
        }

        private async void ExecuteExcluirCommand()
        {
            try
            {
                if (UsuarioAtual.Id_nivel_usuario == 1)
                {
                    if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja excluir o patrimônio?", "Sim", "Não"))
                    {
                        Load = true;
                        Body = false;
                        LoadMessage = "Deletando patrimônio";

                        bool delete = await HeritageAPIService.DeleteAsync(Patrimonio);

                        if (delete)
                        {
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Patrimônio excluido com sucesso");

                            await NavigationService.GoBackAsync();
                        }
                        else
                            await PageDialogService.DisplayAlertAsync("Erro", "Erro ao excluir Patrimônio", "Ok");
                    }
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a exclusão");
            }
            catch (Exception)
            {
                await PageDialogService.DisplayAlertAsync("Erro", "Erro ao excluir patrimônio", "Ok");
            }
            finally
            {
                Load = false;
                Body = true;
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        } 

        #endregion

    }
}
