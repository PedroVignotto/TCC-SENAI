using HeritageV02MVVM.Themes;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HeritageV02MVVM.Utilities
{
    public class Theme
    {
        public string Tema { get; set; }

        public Theme()
        {

            if (Application.Current.Properties.ContainsKey("Theme"))
                Tema = Application.Current.Properties["Theme"] as string;
            else
                Application.Current.Properties.Add("Theme", "Light");

        }

        public void ToExchangeTheme()
        {

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                if (Tema == "Light")
                    mergedDictionaries.Add(new LightTheme());
                else if (Tema == "Dark")
                    mergedDictionaries.Add(new DarkTheme());
            }

        }

    }
}
