using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeritageV02MVVM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExibirGerenciador : ContentPage
    {
        public ExibirGerenciador()
        {
            InitializeComponent();
        }

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (flexNivel.IsVisible == false)
            {
                stkNivel.IsVisible = false;
                flexNivel.IsVisible = true;
            }
            else if (flexNivel.IsVisible == true)
            {
                stkNivel.IsVisible = true;
                flexNivel.IsVisible = false;
            }
        }
    }
}