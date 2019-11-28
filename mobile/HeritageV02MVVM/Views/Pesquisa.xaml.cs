using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace HeritageV02MVVM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Pesquisa : Xamarin.Forms.TabbedPage
    {
        public Pesquisa()
        {
            InitializeComponent();

            //Children.Add(new PesquisarPatrimonio());
            //Children.Add(new PesquisarAmbientes());
            //Children.Add(new PesquisarUsuarios());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
    }
}