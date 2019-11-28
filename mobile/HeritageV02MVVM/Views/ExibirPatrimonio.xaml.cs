using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeritageV02MVVM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExibirPatrimonio : ContentPage
    {
        public ExibirPatrimonio()
        {
            InitializeComponent();
        }

        private void Exibir_Codigo_Tapped(object sender, EventArgs e)
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

        private void Exibir_Descricao_Tapped(object sender, EventArgs e)
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

        private void Exibir_Amb_Tapped(object sender, EventArgs e)
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