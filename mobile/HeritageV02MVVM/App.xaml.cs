using System;
using HeritageV02MVVM.Services;
using HeritageV02MVVM.Services.Abstraction;
using HeritageV02MVVM.ViewModels;
using HeritageV02MVVM.Views;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

            if (Application.Current.Properties.ContainsKey("ManterLogado"))
            {
                manterLogado = Convert.ToBoolean(Application.Current.Properties["ManterLogado"]);
            }

            if (Application.Current.Properties.ContainsKey("Token"))
            {
                string token = Convert.ToString(Application.Current.Properties["Token"]);

                if (token != null)
                {
                    if (manterLogado)
                        await NavigationService.NavigateAsync(new Uri("https://www.Heritge/Menu/NavigationPage/Main", UriKind.Absolute));
                    else
                        await NavigationService.NavigateAsync("Login");
                }
                else
                    await NavigationService.NavigateAsync("Login");
            }
            else
                await NavigationService.NavigateAsync("Login");
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

            containerRegistry.RegisterSingleton<IHeritageAPIService, HeritageAPIService>();
        }
    }
}
