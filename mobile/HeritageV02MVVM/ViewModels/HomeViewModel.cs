using HeritageV02MVVM.Models;
using HeritageV02MVVM.Services.Abstraction;
using HeritageV02MVVM.Utilities;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HeritageV02MVVM.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {

        #region Variáveis

        private int _numeroPatrimonios;

        public int NumeroPatrimonios
        {
            get => _numeroPatrimonios;
            set => SetProperty(ref _numeroPatrimonios, value);
        }

        private int _numeroAmbientes;

        public int NumeroAmbientes
        {
            get => _numeroAmbientes;
            set => SetProperty(ref _numeroAmbientes, value);
        }

        private int _numeroUsuarios;

        public int NumeroUsuarios
        {
            get => _numeroUsuarios; 
            set => SetProperty(ref _numeroUsuarios, value);
        }

        private int _numeroGerenciadores;

        public int NumeroGerenciadores
        {
            get => _numeroGerenciadores;
            set => SetProperty(ref _numeroGerenciadores, value);
        }

        private int _numeroAdm;

        public int NumeroAdm
        {
            get => _numeroAdm;  
            set => SetProperty(ref _numeroAdm, value);
        }

        private int _numeroSuportes;

        public int NumeroSuportes
        {
            get => _numeroSuportes;
            set => SetProperty(ref _numeroSuportes, value);
        }

        #endregion

        public HomeViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base (navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Home";
            Icone = "homeIcon.png";
        }

        #region Métodos

        public override async void Initialize(INavigationParameters parameters)
        {
            UsuarioAtual = new Usuario();
            JsonUsuario = new JsonUsuario();
            Icon = new Icon();

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            try
            {
                ObservableCollection<Patrimonio> patrimonios = await HeritageAPIService.GetAsyncPatrimonios(UsuarioAtual.Id_empresa);
                ObservableCollection<Patrimonio> patrimonios_publish = new ObservableCollection<Patrimonio>();

                NumeroPatrimonios = 0;

                foreach (Patrimonio patrimonio in patrimonios)
                {
                    NumeroPatrimonios++;

                    if (NumeroPatrimonios >= patrimonios.Count - 3)
                    {
                        patrimonios_publish.Add(patrimonio);
                    }
                }

                ObservableCollection<Ambiente> ambientes = await HeritageAPIService.GetAsyncAmbientes(UsuarioAtual.Id_empresa);

                NumeroAmbientes = 0;

                foreach (Ambiente ambiente in ambientes)
                {
                    NumeroAmbientes++;
                }

                ObservableCollection<Usuario> usuarios = await HeritageAPIService.GetAsyncUsuarios(UsuarioAtual.Id_empresa);

                NumeroUsuarios = 0;
                NumeroGerenciadores = 0;
                NumeroAdm = 0;
                NumeroSuportes = 0;

                foreach (Usuario usuario in usuarios)
                {
                    NumeroUsuarios++;

                    if (usuario.Id_nivel_usuario == 2)
                    {
                        NumeroGerenciadores++;
                    }
                    else if (usuario.Id_nivel_usuario == 3)
                    {
                        NumeroSuportes++;
                    }
                    else if (usuario.Id_nivel_usuario == 1)
                    {
                        NumeroAdm++;
                    }
                }

                IsBusy = true;
            }
            catch (Exception ex)
            {
                var param = new DialogParameters
                {
                    { "Message", "Erro ao carregar ultimos patrimônios adicionados" },
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
