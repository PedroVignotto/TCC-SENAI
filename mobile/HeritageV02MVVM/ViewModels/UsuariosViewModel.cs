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
    public class UsuariosViewModel : ViewModelBase
    {

        #region Command

        private DelegateCommand<Usuario> _ExibirUsuarioCommand;
        public DelegateCommand<Usuario> ExibirUsuarioCommand => _ExibirUsuarioCommand ?? (_ExibirUsuarioCommand = new DelegateCommand<Usuario>(async (itemSelect) => await ExecuteExibirUsuarioCommand(itemSelect).ConfigureAwait(false), (itemSelect) => !IsBusy));

        #endregion

        #region Variáveis

        private ObservableCollection<Usuario> usuarios;

        public ObservableCollection<Usuario> Usuarios
        {
            get => usuarios;
            set => SetProperty(ref usuarios, value);
        }

        private bool _isAuthorized;

        public bool IsAuthorized
        {
            get => _isAuthorized;
            set => SetProperty(ref _isAuthorized, value);
        }

        #endregion

        public UsuariosViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {

            Title = "Usuários";
            Icone = "avatarIcon.png";

        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {
            UsuarioAtual = new Usuario();
            Icon = new Icon();
            JsonUsuario = new JsonUsuario();
            Usuarios = new ObservableCollection<Usuario>();

            Load = true;

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            await LoadAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await LoadAsync();
        }

        private async Task ExecuteExibirUsuarioCommand(Usuario usuario)
        {

            var navigationParams = new NavigationParameters
            {
                {"usuario", usuario}
            };

            if (usuario.Id_nivel_usuario == 1)
                await NavigationService.NavigateAsync("ExibirAdministrador", navigationParams);
            else if (usuario.Id_nivel_usuario == 2)
                await NavigationService.NavigateAsync("ExibirGerenciador", navigationParams);
            else if (usuario.Id_nivel_usuario == 3)
                await NavigationService.NavigateAsync("ExibirSuporte", navigationParams);

        }

        private async Task LoadAsync()
        {
            try
            {
                ObservableCollection<Usuario> usuarios = await HeritageAPIService.GetAsyncUsuarios(UsuarioAtual.Id_empresa);

                Usuarios.Clear();

                foreach (Usuario usuario in usuarios)
                {
                    if (UsuarioAtual.Id != usuario.Id)
                        Usuarios.Add(usuario);
                }

                if (Usuarios.Count == 0)
                    Null = false;
                else
                    Body = true;

                IsBusy = true;
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
                Null = true;

                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: Usuários");
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
            
        }

        #endregion

    }
}
