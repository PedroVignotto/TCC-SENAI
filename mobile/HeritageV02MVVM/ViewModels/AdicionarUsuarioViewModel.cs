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
    public class AdicionarUsuarioViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand AdiconarUsuarioCommand { get; private set; }

        #endregion

        #region Variáveis

        private ObservableCollection<Usuario> usuarios;
        public ObservableCollection<Usuario> Usuarios
        {
            get { return usuarios; }
            set { SetProperty(ref usuarios, value); }
        }

        private string _nivel;
        public string Nivel
        {
            get { return _nivel; }
            set => SetProperty(ref _nivel, value);
        }

        private Usuario usuario;
        public Usuario Usuario
        {
            get { return usuario; }
            set { SetProperty(ref usuario, value); }
        }

        private ObservableCollection<string> niveis;
        public ObservableCollection<string> Niveis
        {
            get { return niveis; }
            set { SetProperty(ref niveis, value); }
        }

        #endregion

        public AdicionarUsuarioViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Adicionar usuário";
            AdiconarUsuarioCommand = new DelegateCommand(ExecuteAdicionarUsuarioCommand);
        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {
            Niveis = new ObservableCollection<string>
            {
                "Administrador geral", "Gerenciador de ambiente", "Suporte"
            };

            Usuario = new Usuario();
            Usuarios = new ObservableCollection<Usuario>();
            UsuarioAtual = new Usuario();
            Icon = new Icon();
            JsonUsuario = new JsonUsuario();

            UsuarioAtual = JsonUsuario.GetUsuarioJson();
            Icone = Icon.IconName("avatar");

            Body = true;

            await LoadAsync();
        }

        public async Task LoadAsync()
        {
            try
            {
                ObservableCollection<Usuario> usuarios = await HeritageAPIService.GetAsyncUsuarios();

                Usuarios.Clear();

                foreach (Usuario usuario in usuarios)
                {
                    if (UsuarioAtual.Id != usuario.Id)
                        Usuarios.Add(usuario);
                }

            }
            catch (Exception ex)
            {
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao carregar usuários" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: AdicionarUsuarios");
            }
        }

        private bool Validation(Usuario usuario)
        {
            if (usuario.Email == null || Nivel == null)
            {
                return false;
            }
            else
            {
                foreach (Usuario usuario_foreach in Usuarios)
                {
                    if (Usuario.Email == usuario_foreach.Email)
                    {
                        Usuario = usuario_foreach;
                    }
                }

                return true;
            }
        }

        private async void ExecuteAdicionarUsuarioCommand()
        {
            Load = true;
            Body = false;

            try
            {
                IsBusy = true;

                if (Validation(Usuario))
                {
                    if (Nivel == "Administrador geral")
                        Usuario.Id_nivel_usuario = 1;
                    else if (Nivel == "Gerenciador de ambiente")
                        Usuario.Id_nivel_usuario = 2;
                    else if (Nivel == "Suporte")
                        Usuario.Id_nivel_usuario = 3;

                    Usuario.Id_empresa = UsuarioAtual.Id_empresa;

                    var put = await HeritageAPIService.PutAsync(Usuario);

                    if (put == false)
                    {
                        var param = new DialogParameters
                        {
                            { "Message", "Erro ao adicionar usuário" },
                            { "Title", "Erro" },
                            { "Icon", Icon.IconName("bug") }
                        };

                        DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                    }
                    else
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Usuário adicionado com sucesso");
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Preencha todos os campos");
            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao adicionar usuário" },
                    { "Title", "Erro" },
                    { "Icon", Icon.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Console.WriteLine($"Erro ao adicionar usuário: {ex.Message}, Página: AdicionarUsuario");
            }
            finally
            {
                IsBusy = false;
                Body = true;
                Load = false;
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

        #endregion

    }
}
