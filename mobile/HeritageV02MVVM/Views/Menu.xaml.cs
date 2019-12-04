using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeritageV02MVVM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : MasterDetailPage, IMasterDetailPageOptions
    {
        public Menu()
        {
            InitializeComponent();
        }

        public bool IsPresentedAfterNavigation
        {
            get { return false; }
        }
    }
}