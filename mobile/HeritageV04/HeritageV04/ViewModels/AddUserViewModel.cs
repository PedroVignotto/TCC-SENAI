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
    public class AddUserViewModel : ViewModelBase
    {

        public DelegateCommand AddCommand => new DelegateCommand(ExecuteAddCommand);

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

        #endregion

        public AddUserViewModel(INavigationService navigationService, IHeritageAPIService heritageAPIService, IPageDialogService pageDialogService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            Title = "Adicionar usuário";
        }

        #region Methods
        public override void Initialize(INavigationParameters parameters)
        {
            CurrentUser = new User();
            UserJson = new UserJson();
            IconTheme = new IconTheme();
            User = new User();

            Levels = new ObservableCollection<string>
            {
                "Administrador geral", "Gerenciador de ambiente", "Suporte"
            };

            Body = true;

            CurrentUser = UserJson.GetUsuarioJson();
            Icone = IconTheme.IconName("avatar");

            User.SerializeName = false;
        }

        private bool Validation(User user)
        {
            if (user.Email == null || Level == null)
                return false;
            else
                return true;
        }

        private async void ExecuteAddCommand()
        {
            Load = true;
            Body = false;

            try
            {
                IsBusy = true;

                if (Validation(User))
                {
                    if (Level == "Administrador geral")
                        User.UserLevel = 1;
                    else if (Level == "Gerenciador de ambiente")
                        User.UserLevel = 2;
                    else if (Level == "Suporte")
                        User.UserLevel = 3;

                    User.CompanyId = CurrentUser.CompanyId;
                    User.SerializeName = false;
                    Repost repost = await HeritageAPIService.AddUserAsync(User);

                    if (repost.Success)
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().LongAlert("Usuário adicionado com sucesso");
                        User = new User();
                        Level = null;
                    }
                    else
                    {
                        var param = new DialogParameters
                        {
                            { "Message", repost.ErrorMessage },
                            { "Title", "Erro" },
                            { "Icon", IconTheme.IconName("bug") }
                        };

                        DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                    }
                        
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
                    { "Icon", IconTheme.IconName("bug") }
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
