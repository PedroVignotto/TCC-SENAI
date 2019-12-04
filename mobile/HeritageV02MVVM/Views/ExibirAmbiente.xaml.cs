using HeritageV02MVVM.Models;
using HeritageV02MVVM.Utilities;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeritageV02MVVM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExibirAmbiente : ContentPage
    {
        Usuario Usuario { get; set; }

        public ExibirAmbiente()
        {
            InitializeComponent();

            JsonUsuario jsonUsuario = new JsonUsuario();
            Usuario = jsonUsuario.GetUsuarioJson();

        }

        private void Exibir_Nome_Tapped(object sender, EventArgs e)
        {
            if (Usuario.Id_nivel_usuario == 1)
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
            if (Usuario.Id_nivel_usuario == 1)
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