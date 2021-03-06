﻿using HeritageV04.Themes;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HeritageV04.Utilities
{
    public class Theme
    {
        public string Tema { get; set; }

        public Theme()
        {

            if (Application.Current.Properties.ContainsKey("Theme"))
                Tema = Application.Current.Properties["Theme"] as string;
            else
            {
                Application.Current.Properties.Add("Theme", "Light");
                Tema = "Light";
            }

        }

        public void ToExchangeTheme()
        {

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                if (Tema == "Light")
                {
                    if (Application.Current.Properties.ContainsKey("ThemeActived"))
                        Application.Current.Properties["ThemeActived"] = false;
                    else
                        Application.Current.Properties.Add("ThemeActived", false);

                    mergedDictionaries.Add(new LightTheme());
                }
                else if (Tema == "Dark")
                {

                    if (Application.Current.Properties.ContainsKey("TemaActived"))
                        Application.Current.Properties["TemaActived"] = true;
                    else
                        Application.Current.Properties.Add("TemaActived", true);

                    mergedDictionaries.Add(new DarkTheme());

                }

            }

        }

    }
}
