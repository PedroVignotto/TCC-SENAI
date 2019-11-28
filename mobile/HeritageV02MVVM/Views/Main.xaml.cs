using Plugin.Connectivity;
using Xamarin.Forms;
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

            Children.Add(new Home());
            Children.Add(new Patrimonios());
            Children.Add(new Ambientes());
            Children.Add(new Usuarios());

            //if (!CrossConnectivity.Current.IsConnected)
            //{
            //    Children.Add(new SemInternet());
            //}
            //else
            //{
            //   
            //    
            //    
            //    
            //}
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
    }
}