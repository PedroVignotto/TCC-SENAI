using HeritageV02MVVM.Models;
using HeritageV02MVVM.Services.Abstraction;
using HeritageV02MVVM.Utilities;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HeritageV02MVVM.ViewModels
{
    public class PerfilViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand SalvarCommand { get; private set; }
        public DelegateCommand ExcluirCommand { get; private set; }
        public DelegateCommand PegarFotoCommand { get; private set; }

        #endregion

        #region Variáveis

        private string _iconEmail;

        public string IconEmail
        {
            get => _iconEmail;
            set => SetProperty(ref _iconEmail, value);
        }

        private string _iconNivel;

        public string IconNivel
        {
            get => _iconNivel;
            set => SetProperty(ref _iconNivel, value);
        }

        private string _iconNome;

        public string IconNome
        {
            get => _iconNome;
            set => SetProperty(ref _iconNome, value);
        }
        
        private string _iconExcluir;

        public string IconExcluir
        {
            get => _iconExcluir;
            set => SetProperty(ref _iconExcluir, value);
        }
        
        private string _iconFoto;

        public string IconFoto
        {
            get => _iconFoto;
            set => SetProperty(ref _iconFoto, value);
        }

        #endregion

        public PerfilViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Perfil";
            Icone = "avatarIcon.png";

            SalvarCommand = new DelegateCommand(ExecuteSalvarCommand);
            ExcluirCommand = new DelegateCommand(ExecuteExcluirCommand);
            PegarFotoCommand = new DelegateCommand(ExecutePegarFotoCommand);

        }

        #region Métodos

        public override void Initialize(INavigationParameters parameters)
        {
            UsuarioAtual = new Usuario();
            Icon = new Icon();
            JsonUsuario = new JsonUsuario();

            UsuarioAtual = JsonUsuario.GetUsuarioJson();

            switch (UsuarioAtual.Id_nivel_usuario)
            {
                case 1:
                    UsuarioAtual.Nivel_usuario = "Administrador geral";
                    break;

                case 2:
                    UsuarioAtual.Nivel_usuario = "Gerenciador de ambiente";
                    break;

                case 3:
                    UsuarioAtual.Nivel_usuario = "Suporte";
                    break;
            }

            IconFoto = Icon.IconName("picture");
            IconEmail = Icon.IconName("email");
            IconNivel = Icon.IconName("controls");
            IconExcluir = Icon.IconName("cancel");
            IconNome = Icon.IconName("name");
        }

        private async Task GetPhotoAsync()
        {
            var media = CrossMedia.Current;

            var file = await media.PickPhotoAsync();

            UsuarioAtual.Imagem = file.Path;
        }

        private async void ExecutePegarFotoCommand()
        {
            await GetPhotoAsync();
        }

        private async void ExecuteSalvarCommand()
        {
            if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja salvar as alterações?", "Sim", "Não"))
            {
                bool up = await HeritageAPIService.PutAsync(UsuarioAtual);

                if (up)
                {
                    var param = new DialogParameters
                    {
                        { "Message", "Informações atualizadas com sucesso" },
                        { "Title", "Pronto" },
                        { "Icon", Icon.IconName("verified") }
                    };

                    DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                    JsonUsuario.SetUsuarioJson(UsuarioAtual);
                }
                else
                {
                    DialogParameters param = new DialogParameters
                    {
                        { "Message", "Erro ao atualizar informações" },
                        { "Title", "Erro" },
                        { "Icon", Icon.IconName("bug") }
                    };

                    DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                }
            }
        }

        private async void ExecuteExcluirCommand()
        {
            if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja excluir seu perfil?", "Sim", "Não"))
            {
                bool delete = await HeritageAPIService.DeleteAsync(UsuarioAtual);

                if (delete)
                {
                    DialogParameters param = new DialogParameters
                    {
                        { "Message", "Perfil excluido com sucesso" },
                        { "Title", "Pronto" },
                        { "Icon", Icon.IconName("verified") }
                    };

                    DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                    await NavigationService.NavigateAsync(new Uri("https://www.Heritage.com/Login", UriKind.Absolute));
                }
                else
                {
                    var param = new DialogParameters
                    {
                        { "Message", "Erro ao excluir perfil" },
                        { "Title", "Erro" },
                        { "Icon", Icon.IconName("bug") }
                    };

                    DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                }
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

        public override void Destroy()
        {
            Icon = null;
            UsuarioAtual = null;
            JsonUsuario = null;

            IconFoto = null;
            IconEmail = null;
            IconNivel = null;
            IconExcluir = null;
            IconNome = null;

            ExcluirCommand = null;
            PegarFotoCommand = null;
            SalvarCommand = null;
        }

        #endregion

    }
}
