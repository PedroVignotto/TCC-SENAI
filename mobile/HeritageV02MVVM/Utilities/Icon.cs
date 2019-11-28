using Xamarin.Forms;

namespace HeritageV02MVVM.Utilities
{
    public class Icon
    {
        public string Tema { get; set; }

        public Icon()
        {
            if (Application.Current.Properties.ContainsKey("Theme"))
                Tema = Application.Current.Properties["Theme"] as string;
            else
                Tema = "Light";
        }

        public string IconName(string icon)
        {
            return icon + Tema + ".png";
        }
    }
}
