using HeritageV04.Models;
using HeritageV04.Services.Abstractions;
using HeritageV04.Utilities;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;

namespace HeritageV04.ViewModels
{
    public class DisplaySupportViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand SaveCommand => new DelegateCommand(ExecuteSaveCommand);

        public DelegateCommand DeleteCommand => new DelegateCommand(ExecuteDeleteCommand);

        #endregion

        #region Variables

        private string level;
        public string Level
        {
            get { return level; }
            set => SetProperty(ref level, value);
        }

        private User user;
        public User User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        private ObservableCollection<string> levels;
        public ObservableCollection<string> Levels
        {
            get { return levels; }
            set { SetProperty(ref levels, value); }
        }

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

        private string loadMessage;

        public string LoadMessage
        {
            get { return loadMessage; }
            set => SetProperty(ref loadMessage, value);
        }

        #endregion

        public DisplaySupportViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Suporte";
        }

        #region Methods
        public override void Initialize(INavigationParameters parameters)
        {
            UserJson = new UserJson();
            CurrentUser = new User();
            IconTheme = new IconTheme();

            Level = "Suporte";

            Levels = new ObservableCollection<string>
            {
                "Administrador geral", "Gerenciador de ambiente", "Suporte"
            };

            CurrentUser = UserJson.GetUsuarioJson();

            IconEmail = IconTheme.IconName("email");
            IconLevel = IconTheme.IconName("controls");
            IconDelete = IconTheme.IconName("cancel");
            IconName = IconTheme.IconName("name");
            Icone = IconTheme.IconName("avatar");

            Body = true;
            Load = false;

            if (parameters.ContainsKey("user"))
                User = (User)parameters["user"];
        }

        private async void ExecuteSaveCommand()
        {
            try
            {
                if (CurrentUser.UserLevel == 1)
                {
                    if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja salvar as alterações?", "Sim", "Não"))
                    {
                        Load = true;
                        Body = false;
                        LoadMessage = "Atualizando usuário";

                        if (Level == "Administrador geral")
                            User.UserLevel = 1;
                        else if (Level == "Gerenciador de ambiente")
                            User.UserLevel = 2;
                        else if (Level == "Suporte")
                            User.UserLevel = 3;

                        Repost repost = await HeritageAPIService.PutAsync(User);

                        if (repost.Success)
                        {
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Usuário atualizado com sucesso");
                            await NavigationService.GoBackAsync();
                        }
                        else
                        {
                            DialogParameters param = new DialogParameters
                            {
                                { "Message", "Erro ao atualizar usuário" },
                                { "Title", "Erro" },
                                { "Icon", IconTheme.IconName("bug") }
                            };

                            DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                        }

                    }
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a alteração");
            }
            catch (Exception ex)
            {
                Load = false;
                Body = true;
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao atualizar usuário" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: AdicionarAmbiente");
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

        private async void ExecuteDeleteCommand()
        {
            try
            {
                if (CurrentUser.UserLevel == 1)
                {
                    if (await PageDialogService.DisplayAlertAsync("Aviso", "Tem certeza que deseja excluir o usuario?", "Sim", "Não"))
                    {
                        Load = true;
                        Body = false;
                        LoadMessage = "Excluindo usuário";

                        User.CompanyId = null;
                        User.UserLevel = null;

                        bool delete = await HeritageAPIService.DeleteAsync(User);

                        if (delete)
                        {
                            Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Usuário excluido com sucesso");
                            await NavigationService.GoBackAsync();
                        }
                        else
                        {
                            DialogParameters param = new DialogParameters
                            {
                                { "Message", "Erro ao excluir usuário" },
                                { "Title", "Erro" },
                                { "Icon", IconTheme.IconName("bug") }
                            };

                            DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                        }
                    }
                }
                else
                    Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Seu nível de usuário não permite a exclusão");
            }
            catch (Exception ex)
            {
                Load = false;
                Body = true;
                DialogParameters param = new DialogParameters
                {
                    { "Message", "Erro ao excluir usuário" },
                    { "Title", "Erro" },
                    { "Icon", IconTheme.IconName("bug") }
                };

                DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: AdicionarAmbiente");
            }
            finally
            {
                Load = false;
                Body = true;
            }
        } 
        #endregion

    }
}
