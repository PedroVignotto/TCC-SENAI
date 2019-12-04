using HeritageV02MVVM.Models;
using HeritageV02MVVM.Utilities;
using Plugin.Connectivity;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace HeritageV02MVVM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : Xamarin.Forms.TabbedPage
    {
        public Main()
        {
            InitializeComponent();

            JsonUsuario jsonUsuario = new JsonUsuario();

            Usuario usuario = jsonUsuario.GetUsuarioJson();

            if (!CrossConnectivity.Current.IsConnected)
            {
                Children.Add(new SemInternet());
            }
            else if(usuario.Id_nivel_usuario == 1 || usuario.Id_nivel_usuario == 2)
            {
                Children.Add(new Home());
                Children.Add(new Patrimonios());
                Children.Add(new Ambientes());
                Children.Add(new Usuarios());
            }
            else
            {
                Children.Add(new Home());
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
    }
}