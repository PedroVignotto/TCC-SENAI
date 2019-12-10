using HeritageV04.Services;
using HeritageV04.Services.Abstractions;
using HeritageV04.ViewModels;
using HeritageV04.Views;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HeritageV04
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

            bool login = false;

            if (Application.Current.Properties.ContainsKey("Login"))
                login = Convert.ToBoolean(Application.Current.Properties["Login"]);

            if (login == true)
                await NavigationService.NavigateAsync("Menu/NavigationPage/Main");
            else
                await NavigationService.NavigateAsync("Login");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Login, LoginViewModel>();


            containerRegistry.RegisterDialog<DialogPage, DialogPageViewModel>();

            containerRegistry.RegisterSingleton<IHeritageAPIService, HeritageAPIService>();

        }
    }
}
