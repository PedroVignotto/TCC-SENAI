using HeritageV02MVVM.Events;
using HeritageV02MVVM.Models;
using HeritageV02MVVM.Services.Abstraction;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeritageV02MVVM.ViewModels
{
    public class PesquisarUsuariosViewModel : ViewModelBase
    {

        #region Commands

        private DelegateCommand<Usuario> _ExibirUsuarioCommand;
        public DelegateCommand<Usuario> ExibirUsuarioCommand => _ExibirUsuarioCommand ?? (_ExibirUsuarioCommand = new DelegateCommand<Usuario>(async (itemSelect) => await ExecuteExibirUsuarioCommand(itemSelect), (itemSelect) => !IsBusy));

        #endregion

        #region Variáveis

        IEventAggregator _eventAggregator;

        private ObservableCollection<Usuario> usuario;

        public ObservableCollection<Usuario> Usuarios
        {
            get { return usuario; }
            set => SetProperty(ref usuario, value);
        }

        #endregion

        public PesquisarUsuariosViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IHeritageAPIService heritageAPIService, IDialogService dialogService, IEventAggregator eventAggregator) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Usuários";
            Icone = "avatarIcon.png";

            _eventAggregator = eventAggregator;

            Body = true;

            Usuarios = new ObservableCollection<Usuario>();

            _eventAggregator.GetEvent<PesquisarUsuariosEvent>().Subscribe(PesquisarUsuario);
        }

        public void PesquisarUsuario(ObservableCollection<Usuario> usuarios)
        {
            Usuarios.Clear();

            foreach (Usuario usuario in usuarios)
                Usuarios.Add(usuario);
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

    }
}
