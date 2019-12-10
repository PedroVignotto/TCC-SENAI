using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeritageV04.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            InitializeComponent();
        }

        private void Exibir_Email_Tapped(object sender, EventArgs e)
        {
            if (flexEmail.IsVisible == false)
            {
                stkEmail.IsVisible = false;
                flexEmail.IsVisible = true;
            }
            else if (flexEmail.IsVisible == true)
            {
                stkEmail.IsVisible = true;
                flexEmail.IsVisible = false;
            }
        }

        private void Exibir_Nome_Tapped(object sender, EventArgs e)
        {
            if (flexNome.IsVisible == false)
            {
                stkNome.IsVisible = false;
                flexNome.IsVisible = true;
            }
            else if (flexNome.IsVisible == true)
            {
                stkNome.IsVisible = true;
                flexNome.IsVisible = false;
            }
        }
    }
}