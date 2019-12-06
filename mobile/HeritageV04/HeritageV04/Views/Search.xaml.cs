using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace HeritageV04.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Search : Xamarin.Forms.TabbedPage
    {
        public Search()
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