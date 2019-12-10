using HeritageV04.Models;
using HeritageV04.Utilities;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeritageV04.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayEnvironment : ContentPage
    {
        User User { get; set; }

        public DisplayEnvironment()
        {
            InitializeComponent();

            UserJson userJson = new UserJson();
            User = userJson.GetUsuarioJson();

        }

        private void Exibir_Nome_Tapped(object sender, EventArgs e)
        {
            if (User.UserLevel == 1)
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

        private void Exibir_Adm_Tapped(object sender, EventArgs e)
        {
            if (User.UserLevel == 1)
            {
                if (flexAdm.IsVisible == false)
                {
                    sktAdm.IsVisible = false;
                    flexAdm.IsVisible = true;
                }
                else if (flexAdm.IsVisible == true)
                {
                    sktAdm.IsVisible = true;
                    flexAdm.IsVisible = false;
                }
            }
        }
    }
}