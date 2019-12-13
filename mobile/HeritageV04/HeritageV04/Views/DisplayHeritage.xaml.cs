using HeritageV04.Models;
using HeritageV04.Utilities;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeritageV04.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisplayHeritage : ContentPage
    {
        User User { get; set; }

        public DisplayHeritage()
        {
            InitializeComponent();

            UserJson jsonUsuario = new UserJson();
            User = jsonUsuario.GetUsuarioJson();

        }

        private void Exibir_Descricao_Tapped(object sender, EventArgs e)
        {
            if (User.UserLevel == 1)
            {
                if (flexDesc.IsVisible == false)
                {
                    flexDesc.IsVisible = true;
                    stkDesc.IsVisible = false;
                    tbvPatr.HeightRequest = 730;
                }
                else if (flexDesc.IsVisible == true)
                {
                    flexDesc.IsVisible = false;
                    stkDesc.IsVisible = true;
                    tbvPatr.HeightRequest = 430;
                }
            }
        }

        private void Exibir_Amb_Tapped(object sender, EventArgs e)
        {
            if (User.UserLevel == 1)
            {
                if (flexAmb.IsVisible == false)
                {
                    flexAmb.IsVisible = true;
                    stkAmb.IsVisible = false;
                }
                else if (flexAmb.IsVisible == true)
                {
                    flexAmb.IsVisible = false;
                    stkAmb.IsVisible = true;
                }
            }
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
    }
}