using HeritageV02MVVM.Models;
using HeritageV02MVVM.Services.Abstraction;
using HeritageV02MVVM.Utilities;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using Xamarin.Forms;

namespace HeritageV02MVVM.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        #region Commands

        public DelegateCommand LoginCommand { get; private set; }

        #endregion

        #region Váriaveis

        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        private Usuario usuario;

        public Usuario Usuario
        {
            get => usuario;
            set => SetProperty(ref usuario, value);
        }

        #endregion

        public LoginViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IHeritageAPIService heritageAPIService, IDialogService dialogService) : base(navigationService, heritageAPIService, pageDialogService, dialogService)
        {
            LoginCommand = new DelegateCommand(ExecuteLoginCommand);
        }

        #region Método

        public override void Initialize(INavigationParameters parameters)
        {
            Usuario = new Usuario();
            JsonUsuario = new JsonUsuario();

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
                        { "Icon", Icon.IconName("bug") }
                    };

                    DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                }
                else
                {
                    Usuario = await HeritageAPIService.Login(Usuario);

                    if (Usuario == null)
                    {
                        DialogParameters param = new DialogParameters
                        {
                            { "Message", "Email ou senha incorretos" },
                            { "Title", "Erro" },
                            { "Icon", Icon.IconName("bug") }
                        };

                        DialogService.ShowDialog("DialogPage", param, CloseDialogCallback);
                    }
                    else
                    {

                        if (Application.Current.Properties.ContainsKey("Login"))
                            Application.Current.Properties["Login"] = IsChecked;
                        else
                            Application.Current.Properties.Add("Login", IsChecked);

                        Token.SetToken(Usuario.Token);
                        JsonUsuario.SetUsuarioJson(Usuario);

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

        public override void Destroy()
        {
            Icon = null;
            UsuarioAtual = null;
            Usuario = null;
            JsonUsuario = null;
        }

        #endregion
    }
}
