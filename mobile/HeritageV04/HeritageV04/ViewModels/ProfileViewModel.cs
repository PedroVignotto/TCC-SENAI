using HeritageV04.Models;
using HeritageV04.Services.Abstractions;
using HeritageV04.Utilities;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace HeritageV04.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand SaveCommand => new DelegateCommand(ExecuteSaveCommand);
        public DelegateCommand DeleteCommand => new DelegateCommand(ExecuteDeleteCommand);
        public DelegateCommand GetPhotoCommand => new DelegateCommand(ExecuteGetPhotoCommand);

        #endregion

        #region Variables

        private string iconEmail;

        public string IconEmail
        {
            get => iconEmail;
            set => SetProperty(ref iconEmail, value);
        }

        private string iconLevel;

        public string IconLevel
        {
            get => iconLevel;
            set => SetProperty(ref iconLevel, value);
        }

        private string iconName;

        public string IconName
        {
            get => iconName;
            set => SetProperty(ref iconName, value);
        }

        private string iconDelete;

        public string IconDelete
        {
            get => iconDelete;
            set => SetProperty(ref iconDelete, value);
        }

        private string iconPhoto;
        public string IconPhoto
        {
            get { return iconPhoto; }
            set { SetProperty(ref iconPhoto, value); }
        }

        #endregion

        public ProfileViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Perfil";
            Icone = "avatarIcon.png";
        }

        #region Methods

        public override void Initialize(INavigationParameters parameters)
        {
            CurrentUser = new User();
            UserJson = new UserJson();
            IconTheme = new IconTheme();

            CurrentUser = UserJson.GetUsuarioJson();

            switch (CurrentUser.UserLevel)
            {
                case 1:
                    CurrentUser.UserLevelDescription = "Administrador";
                    break;

                case 2:
                    CurrentUser.UserLevelDescription = "Gerenciador";
                    break;

                case 3:
                    CurrentUser.UserLevelDescription = "Suporte";
                    break;
            }

            IconEmail = IconTheme.IconName("email");
            IconLevel = IconTheme.IconName("controls");
            IconDelete = IconTheme.IconName("cancel");
            IconName = IconTheme.IconName("name");
            IconPhoto = IconTheme.IconName("picture");
        }

        private async Task GetPhotoAsync()
        {
            IMedia media = CrossMedia.Current;

            MediaFile file = await media.PickPhotoAsync();

            CurrentUser.Avatar = new Avatar();

            if (file != null)
                CurrentUser.Avatar.Url = file.Path;
            
        }

        private async void ExecuteGetPhotoCommand()
        {
            await GetPhotoAsync();
        }

        private async void ExecuteSaveCommand()
        {
            if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja salvar as alterações?", "Sim", "Não"))
            {
                Repost repost = await HeritageAPIService.PutAsync(CurrentUser);

                if (repost.Success)
                {
                    var param = new DialogParameters
                    {
                        { "Message", "Informações atualizadas com sucesso" },
                        { "Title", "Pronto" },
                        { "Icon", IconTheme.IconName("verified") }
                    };

                    DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);

                    UserJson.SetUsuarioJson(CurrentUser);
                }
                else
                {
                    DialogParameters param = new DialogParameters
                    {
                        { "Message", "Erro ao atualizar informações" },
                        { "Title", "Erro" },
                        { "Icon", IconTheme.IconName("bug") }
                    };

                    DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                }
            }
        }

        private async void ExecuteDeleteCommand()
        {
            if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja excluir seu perfil?", "Sim", "Não"))
            {
                bool delete = await HeritageAPIService.DeleteAsync(CurrentUser);

                if (delete)
                {
                    DialogParameters param = new DialogParameters
                    {
                        { "Message", "Perfil excluido com sucesso" },
                        { "Title", "Pronto" },
                        { "Icon", IconTheme.IconName("verified") }
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
                        { "Icon", IconTheme.IconName("bug") }
                    };

                    DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                }
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        } 
        #endregion
    }
}
