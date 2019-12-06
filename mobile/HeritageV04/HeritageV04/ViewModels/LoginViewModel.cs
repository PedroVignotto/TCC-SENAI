using HeritageV04.Models;
using HeritageV04.Services.Abstractions;
using HeritageV04.Utilities;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using Xamarin.Forms;

namespace HeritageV04.ViewModels
{
    public  class LoginViewModel : ViewModelBase
    {

        public DelegateCommand LoginCommand => new DelegateCommand(ExecuteLoginCommand);

        private User user;
        public User User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set { SetProperty(ref isChecked, value); }
        }

        public LoginViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IHeritageAPIService heritageAPIService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            if (Application.Current.Properties.ContainsKey("Login"))
                Application.Current.Properties["Login"] = null;
            else
                Application.Current.Properties.Add("Login", null);

            if (Application.Current.Properties.ContainsKey("Token"))
                Application.Current.Properties["Token"] = null;
            else
                Application.Current.Properties.Add("Token", null);

            if (Application.Current.Properties.ContainsKey("Usuario"))
                Application.Current.Properties["Usuario"] = null;
            else
                Application.Current.Properties.Add("Usuario", null);

            if (Application.Current.Properties.ContainsKey("Theme"))
                Application.Current.Properties["Theme"] = "Light";
            else
                Application.Current.Properties.Add("Theme", "Light");

        }

        public override void Initialize(INavigationParameters parameters)
        {
            User = new User();
            UserJson = new UserJson();
            IconTheme = new IconTheme();

            Body = true;
        }

        private async void ExecuteLoginCommand()
        {
            try
            {
                Body = false;
                Load = true;

                if (!CrossConnectivity.Current.IsConnected)
                {
                    DialogParameters param = new DialogParameters
                    {
                        { "Message", "Sem conexão com a internet" },
                        { "Title", "Erro" },
                        { "Icon", IconTheme.IconName("bug") }
                    };

                    DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                }
                else
                {
                    User = await HeritageAPIService.UserLogin(User);

                    if (User == null)
                    {
                        DialogParameters param = new DialogParameters
                        {
                            { "Message", "Email ou senha incorretos" },
                            { "Title", "Erro" },
                            { "Icon", IconTheme.IconName("bug") }
                        };

                        DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                    }
                    else
                    {

                        if (Application.Current.Properties.ContainsKey("Login"))
                            Application.Current.Properties["Login"] = IsChecked;
                        else
                            Application.Current.Properties.Add("Login", IsChecked);

                        UserJson.SetUsuarioJson(User);

                        await NavigationService.NavigateAsync(new Uri("https://www.Heritage/Menu/NavigationPage/Main", UriKind.Absolute));

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, Página: Login");
            }
            finally
            {
                Body = true;
                Load = false;
            }
        }

        void CloseDialogCallback(IDialogResult dialogResult)
        {

        }

    }
}
