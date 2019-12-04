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
    public class ExibirAmbienteViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand SalvarCommand => new DelegateCommand(ExecuteSalvarCommand);

        public DelegateCommand VerificacaoCommand => new DelegateCommand(ExecuteVerificacaoCommand);

        public DelegateCommand ExcluirCommand => new DelegateCommand(ExecuteExcluirCommand);

        private DelegateCommand<Patrimonio> _ExibirPatrimonioCommand;
        public DelegateCommand<Patrimonio> ExibirPatrimonioCommand => _ExibirPatrimonioCommand ?? (_ExibirPatrimonioCommand = new DelegateCommand<Patrimonio>(async (itemSelect) => await ExecuteExibirPatrimonioCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variáveis

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

        private Ambiente ambiente;
        public Ambiente Ambiente
        {
            get { return ambiente; }
            set { SetProperty(ref ambiente, value); }
        }

        private ObservableCollection<Patrimonio> patrimonios;
        public ObservableCollection<Patrimonio> Patrimonios
        {
            get { return patrimonios; }
            set { SetProperty(ref patrimonios, value); }
        }

        private Usuario usuario;

        public Usuario Usuario
        {
            get { return usuario; }
            set => SetProperty(ref usuario, value);
        }

        private ObservableCollection<Usuario> usuarios;

        public ObservableCollection<Usuario> Usuarios
        {
            get { return usuarios; }
            set => SetProperty(ref usuarios, value);
        }

        private string _loadMessage;

        public string LoadMessage
        {
            get { return _loadMessage; }
            set => SetProperty(ref _loadMessage, value);
        }

        private bool _bodyPatrimonios;

        public bool BodyPatrimonios
        {
            get { return _bodyPatrimonios; }
            set => SetProperty(ref _bodyPatrimonios, value);
        }

        private bool _loadPatrimonios;

        public bool LoadPatrimonios
        {
            get { return _loadPatrimonios; }
            set => SetProperty(ref _loadPatrimonios, value);
        }

        private bool _nullPatrimonios;

        public bool NullPatrimonios
        {
            get { return _nullPatrimonios; }
            set => SetProperty(ref _nullPatrimonios, value);
        }

        private bool _bodyMovimentacoes;

        public bool BodyMovimentacoes
        {
            get { return _bodyMovimentacoes; }
            set => SetProperty(ref _bodyMovimentacoes, value);
        }

        private bool _loadMovimentacoes;

        public bool LoadMovimentacoes
        {
            get { return _loadMovimentacoes; }
            set => SetProperty(ref _loadMovimentacoes, value);
        }

        private bool _nullMovimentacoes;

        public bool NullMovimentacoes
        {
            get { return _nullMovimentacoes; }
            set => SetProperty(ref _nullMovimentacoes, value);
        }

        private bool _bodyManutencoes;

        public bool BodyManutencoes
        {
            get { return _bodyManutencoes; }
            set => SetProperty(ref _bodyManutencoes, value);
        }

        private bool _loadManutencoes;

        public bool LoadManutencoes
        {
            get { return _loadManutencoes; }
            set => SetProperty(ref _loadManutencoes, value);
        }

        private bool _nullManutencoes;

        public bool NullManutencoes
        {
            get { return _nullManutencoes; }
            set => SetProperty(ref _nullManutencoes, value);
        }

        private string _iconPatrimonio;
        public string IconPatrimonio
        {
            get { return _iconPatrimonio; }
            set { SetProperty(ref _iconPatrimonio, value); }
        }

        private string _iconExcluir;
        public string IconExcluir
        {
            get => _iconExcluir;
            set { SetProperty(ref _iconExcluir, value); }
        }

        private string _iconNome;
        public string IconNome
        {
            get { return _iconNome; }
            set { SetProperty(ref _iconNome, value); }
        }

        private string _iconUsuario;
        public string IconUsuario
        {
            get { return _iconUsuario; }
            set { SetProperty(ref _iconUsuario, value); }
        }

        private string _iconVerificacao;
        public string IconVerificacao
        {
            get { return _iconVerificacao; }
            set { SetProperty(ref _iconVerificacao, value); }
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

        private int _heightListViewPatrimonios;

        public int HeightListViewPatrimonios
        {
            get { return _heightListViewPatrimonios; }
            set => SetProperty(ref _heightListViewPatrimonios, value);
        }

        #endregion

        public ExibirAmbienteViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Exibir ambiente";
        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {

            Patrimonios = new ObservableCollection<Patrimonio>();
            Ambiente = new Ambiente();
            Usuarios = new ObservableCollection<Usuario>();
            Manutencoes = new ObservableCollection<Historico.Manutencao>();
            Movimentacoes = new ObservableCollection<Historico.Movimentacao>();
            UsuarioAtual = new Usuario();
            JsonUsuario = new JsonUsuario();
            Icon = new Icon();

            if (parameters.ContainsKey("ambiente"))
                Ambiente = (Ambiente)parameters["ambiente"];

            Icone = Icon.IconName("placeholder");
            IconUsuario = Icon.IconName("avatar");
            IconPatrimonio = Icon.IconName("box");
            IconExcluir = Icon.IconName("cancel");
            IconNome = Icon.IconName("name");
            IconVerificacao = Icon.IconName("verified");

            Body = true;

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

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

                        LoadMessage = "Atualizando ambientes";
                        Ambiente.Id_usuario = Usuario.Id;

                        bool up = await HeritageAPIService.PutAsync(Ambiente);

                        if (up)
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Ambiente atualizado com sucesso");
                        else
                            await PageDialogService.DisplayAlertAsync("Erro", "Erro ao alterar ambiente", "Ok");
                    }
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a alteração");
            }
            catch (Exception)
            {
                Load = false;
                Body = true;
                await PageDialogService.DisplayAlertAsync("Erro", "Erro ao alterar ambiente", "Ok");
            }
        }

        private async void ExecuteExcluirCommand()
        {
            try
            {
                if (UsuarioAtual.Id_nivel_usuario == 1)
                {
                    if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja excluir o ambiente?", "Sim", "Não"))
                    {
                        Load = true;
                        Body = false;
                        LoadMessage = "Deletando ambientes";

                        bool delete = await HeritageAPIService.DeleteAsync(Ambiente);

                        if (delete)
                        {
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Ambiente excluido com sucesso");
                            await NavigationService.GoBackAsync();
                        }
                        else
                            await PageDialogService.DisplayAlertAsync("Erro", "Erro ao excluir ambiente", "Ok");
                    }
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a exclusão");
            }
            catch (Exception)
            {
                await PageDialogService.DisplayAlertAsync("Erro", "Erro ao excluir ambiente", "Ok");
            }
            finally
            {
                Load = false;
                Body = true;
            }
        }

        private async void ExecuteVerificacaoCommand()
        {
            Body = false;
            Load = true;
            LoadMessage = "Preparando tudo para a verificação";

            foreach (Patrimonio patrimonio in Patrimonios)
            {
                patrimonio.Estado_patrimonio = false;
                await Verficar(patrimonio);
            }

            var navigationParams = new NavigationParameters
            {
                {"patrimonios", Patrimonios}
            };

            await NavigationService.NavigateAsync("PatrimoniosSelect", navigationParams);

            Body = true;
            Load = false;
        }

        private async Task<bool> Verficar(Patrimonio patrimonio)
        {
            bool up = false;

            try
            {
                if (UsuarioAtual.Id_nivel_usuario == 0)
                {
                    Load = true;
                    Body = false;
                    LoadMessage = "Atualizando patrimônio";

                    up = await HeritageAPIService.PutAsync(patrimonio);

                    if (up)
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Patrimônio verificado com sucesso");
                    else
                        await PageDialogService.DisplayAlertAsync("Erro", "Erro ao verificar patrimônio", "Ok");
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a verificação");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return up;
        }

        private async Task LoadAsync()
        {

            BodyManutencoes = false;
            BodyMovimentacoes = false;
            BodyPatrimonios = false;

            LoadManutencoes = true;
            LoadMovimentacoes = true;
            LoadPatrimonios = true;

            try
            {
                IsBusy = true;

                var movimentacoes = await HeritageAPIService.GetAsyncMovimentacoesAmbiente(UsuarioAtual.Id_empresa, Ambiente.Id);

                foreach (Historico.Movimentacao movimentacao in movimentacoes)
                    Movimentacoes.Add(movimentacao);

                if (Movimentacoes.Count > 0)
                {
                    BodyMovimentacoes = true;
                    HeightListViewMovimentacoes = Movimentacoes.Count * 145;
                }
                else
                    NullMovimentacoes = true;
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
                LoadMovimentacoes = false;
            }

            try
            {
                IsBusy = true;

                ObservableCollection<Usuario> usuarios = await HeritageAPIService.GetAsyncUsuarios(UsuarioAtual.Id_empresa);

                Usuarios.Clear();

                foreach (Usuario usuario in usuarios)
                    Usuarios.Add(usuario);
            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar usuários" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                throw ex;
            }

            try
            {
                IsBusy = true;

                ObservableCollection<Patrimonio> patrimonios = await HeritageAPIService.GetAsyncPatrimonios(UsuarioAtual.Id_empresa);

                foreach (var patrimonio in patrimonios)
                {
                    if (patrimonio.Id_ambiente == Ambiente.Id)
                        Patrimonios.Add(patrimonio);
                }

                if (Patrimonios.Count > 0)
                {
                    HeightListViewPatrimonios = Patrimonios.Count * 150;
                    BodyPatrimonios = true;
                }
                else
                    NullPatrimonios = true;
            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar patrimônios" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                NullPatrimonios = true;
                throw ex;
            }
            finally
            {
                IsBusy = false;
                LoadPatrimonios = false;
            }

            try
            {
                IsBusy = true;

                ObservableCollection<Historico.Manutencao> manutencaos = await HeritageAPIService.GetAsyncManutencoesAmbiente(UsuarioAtual.Id_empresa, Ambiente.Id);

                foreach (var manutencao in manutencaos)
                    Manutencoes.Add(manutencao);

                if (Manutencoes.Count > 0)
                {
                    HeightListViewManutencoes = Manutencoes.Count * 120;
                    BodyManutencoes = true;
                }
                else
                    NullManutencoes = true;

            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar patrimônios" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                NullManutencoes = true;
                throw ex;
            }
            finally
            {
                IsBusy = false;
                LoadManutencoes = false;
            }

            try
            {
                IsBusy = true;

                Usuario = new Usuario();
                if (Ambiente.Id_usuario != null)
                {
                    Usuario user = await HeritageAPIService.GetAsyncUsuario(Ambiente.Id_usuario);
                    Usuario = user;
                }
                else
                    Usuario.Name = "Sem gerenciador";
            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar gerenciador" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                throw ex;
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

        #endregion

    }
}
