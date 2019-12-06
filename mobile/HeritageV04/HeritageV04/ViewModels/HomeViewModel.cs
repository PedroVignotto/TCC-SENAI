using HeritageV04.Models;
using HeritageV04.Services.Abstractions;
using HeritageV04.Utilities;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HeritageV04.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {

        #region Variáveis

        private int _numberHeritages;

        public int NumberHeritages
        {
            get => _numberHeritages;
            set => SetProperty(ref _numberHeritages, value);
        }

        private int _numberEnvironments;

        public int NumberEnvironments
        {
            get => _numberEnvironments;
            set => SetProperty(ref _numberEnvironments, value);
        }

        private int _numberUsers;

        public int NumberUsers
        {
            get => _numberUsers;
            set => SetProperty(ref _numberUsers, value);
        }

        private int _numberManager;

        public int NumberManager
        {
            get => _numberManager;
            set => SetProperty(ref _numberManager, value);
        }

        private int _numberAdmin;

        public int NumberAdmin
        {
            get => _numberAdmin;
            set => SetProperty(ref _numberAdmin, value);
        }

        private int _numberSupport;

        public int NumberSupport
        {
            get => _numberSupport;
            set => SetProperty(ref _numberSupport, value);
        }

        #endregion

        public HomeViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Home";
            Icone = "homeIcon.png";
        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {
            CurrentUser = new User();
            UserJson = new UserJson();
            IconTheme = new IconTheme();
            CurrentUser = UserJson.GetUsuarioJson();

            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                ObservableCollection<Heritage> heritages = await HeritageAPIService.GetAsyncHeritages(CurrentUser.CompanyId);
                ObservableCollection<Heritage> heritages_publish = new ObservableCollection<Heritage>();

                NumberHeritages = 0;

                foreach (Heritage heritage in heritages)
                {
                    NumberHeritages++;

                    if (NumberHeritages >= heritages.Count - 3)
                        heritages_publish.Add(heritage);
                }

                ObservableCollection<Ambiente> ambientes = await HeritageAPIService.GetAsyncAmbientes(UsuarioAtual.Id_empresa);

                NumeroAmbientes = 0;

                foreach (Ambiente ambiente in ambientes)
                    NumeroAmbientes++;

                ObservableCollection<Usuario> usuarios = await HeritageAPIService.GetAsyncUsuarios(UsuarioAtual.Id_empresa);

                NumeroUsuarios = 0;
                NumeroGerenciadores = 0;
                NumeroAdm = 0;
                NumeroSuportes = 0;

                foreach (Usuario usuario in usuarios)
                {
                    NumeroUsuarios++;

                    if (usuario.Id_nivel_usuario == 2)
                        NumeroGerenciadores++;
                    else if (usuario.Id_nivel_usuario == 3)
                        NumeroSuportes++;
                    else if (usuario.Id_nivel_usuario == 1)
                        NumeroAdm++;
                }

                IsBusy = true;
            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar algumas informções" },
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
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

        public override void Destroy()
        {
            UsuarioAtual = null;
            JsonUsuario = null;
            Icon = null;
            UsuarioAtual = null;
        }

        #endregion

    }
}
