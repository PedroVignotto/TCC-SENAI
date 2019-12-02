using System;
using HeritageV02MVVM.Models;
using HeritageV02MVVM.Services;
using HeritageV02MVVM.Services.Abstraction;
using HeritageV02MVVM.ViewModels;
using HeritageV02MVVM.Views;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using HeritageV02MVVM.Utilities;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HeritageV02MVVM
{
    [AutoRegisterForNavigation]
    public partial class App : PrismApplication
    {
        public App() : this(null)
        {

        }

        public App(IPlatformInitializer initializer) : this(initializer, true)
        {

        }

        public App(IPlatformInitializer initializer, bool setFormsDependencyResolver) : base(initializer, setFormsDependencyResolver)
        {

        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            bool manterLogado = false;
            IHeritageAPIService heritageAPIService = new HeritageAPIService();

            if (Application.Current.Properties.ContainsKey("Login"))
                manterLogado = Convert.ToBoolean(Application.Current.Properties["Login"]);

            await NavigationService.NavigateAsync("Login");


            //if (Application.Current.Properties.ContainsKey("Token"))
            //{
            //    RequestToken requestToken = new RequestToken();
            //    Token token = requestToken.GetToken();

            //    if (token.Token_acesso != null)
            //    {
            //        JsonUsuario jsonUsuario = new JsonUsuario();
            //        Usuario usuario = jsonUsuario.GetUsuarioJson();

            //        DateTime dateTime = DateTime.Now;

            //        usuario.Token = token.Token_acesso;

            //        if (dateTime > token.Hora_Registro)
            //        {
            //            usuario = await heritageAPIService.Refresh(usuario);

            //            if (usuario == null || manterLogado == false)
            //                await NavigationService.NavigateAsync(new Uri("https://www.Heritage.com/Login", UriKind.Absolute));
            //            else if(usuario != null && manterLogado == true)
            //                jsonUsuario.SetUsuarioJson(usuario);
            //        }

            //    }
            //    else
            //        await NavigationService.NavigateAsync("Login");
            //}
            //else
            //    await NavigationService.NavigateAsync("Login");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.RegisterForNavigation<Login, LoginViewModel>();
            containerRegistry.RegisterForNavigation<Views.Menu, MenuViewModel>();
            containerRegistry.RegisterForNavigation<Main, MainViewModel>();
            containerRegistry.RegisterForNavigation<Home, HomeViewModel>();
            containerRegistry.RegisterForNavigation<Patrimonios, PatrimoniosViewModel>();
            containerRegistry.RegisterForNavigation<Ambientes, AmbientesViewModel>();
            containerRegistry.RegisterForNavigation<Usuarios, UsuariosViewModel>();
            containerRegistry.RegisterForNavigation<Perfil, PerfilViewModel>();
            containerRegistry.RegisterForNavigation<Configuracoes, ConfiguracoesViewModel>();
            containerRegistry.RegisterForNavigation<Pesquisar, PesquisarViewModel>();
            containerRegistry.RegisterForNavigation<PesquisarPatrimonios, PesquisarPatrimoniosViewModel>();

            containerRegistry.RegisterDialog<DialogPage, DialogPageViewModel>();

            containerRegistry.RegisterSingleton<IHeritageAPIService, HeritageAPIService>();
        }
    }
}
