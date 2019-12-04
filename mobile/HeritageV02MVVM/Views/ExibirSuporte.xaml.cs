using HeritageV02MVVM.Models;
using HeritageV02MVVM.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeritageV02MVVM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExibirSuporte : ContentPage
    {
        Usuario Usuario { get; set; }

        public ExibirSuporte()
        {
            InitializeComponent();

            JsonUsuario jsonUsuario = new JsonUsuario();
            Usuario = jsonUsuario.GetUsuarioJson();

        }

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (Usuario.Id_nivel_usuario == 1)
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
}