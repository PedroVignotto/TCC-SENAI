using HeritageV04.Models;
using HeritageV04.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeritageV04.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplaySupport : ContentPage
    {
        User User { get; set; }

        public DisplaySupport()
        {
            InitializeComponent();

            UserJson userJson = new UserJson();
            User = userJson.GetUsuarioJson();

        }

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            if (User.UserLevel == 1)
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