using Xamarin.Forms;

namespace HeritageV04.Utilities
{
    public class IconTheme
    {
        public string Tema { get; set; }

        public IconTheme()
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
