using HeritageV04.Models;
using Plugin.Connectivity;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace HeritageV04.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : Xamarin.Forms.TabbedPage
    {
        public Main()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
    }
}