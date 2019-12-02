using HeritageV02MVVM.Models;
using HeritageV02MVVM.Utilities;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeritageV02MVVM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExibirPatrimonio : ContentPage
    {
        Usuario Usuario { get; set; }

        public ExibirPatrimonio()
        {
            InitializeComponent();

            JsonUsuario jsonUsuario = new JsonUsuario();
            Usuario = jsonUsuario.GetUsuarioJson();

        }

        private void Exibir_Codigo_Tapped(object sender, EventArgs e)
        {
            if (Usuario.Id_nivel_usuario == 1)
            {
                if (flexCod.IsVisible == false)
                {
                    flexCod.IsVisible = true;
                    stkCod.IsVisible = false;
                }
                else if (flexCod.IsVisible == true)
                {
                    flexCod.IsVisible = false;
                    stkCod.IsVisible = true;
                } 
            }
        }

        private void Exibir_Descricao_Tapped(object sender, EventArgs e)
        {
            if (Usuario.Id_nivel_usuario == 1)
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
            if (Usuario.Id_nivel_usuario == 1)
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
    }
}