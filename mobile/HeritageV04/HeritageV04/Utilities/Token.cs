using Newtonsoft.Json;
using Xamarin.Forms;

namespace HeritageV04.Utilities
{
    public class Token
    {
        public string GetToken()
        {
            string token;

            if (Application.Current.Properties.ContainsKey("Token"))
            {
                string data = Application.Current.Properties["Token"] as string;
                token = JsonConvert.DeserializeObject<string>(data);
            }
            else
                token = null;

            return token;
        }

        public bool SetToken(string token)
        {
            bool set = true;

            try
            {

                string data = JsonConvert.SerializeObject(token);
                if (Application.Current.Properties.ContainsKey("Token"))
                    Application.Current.Properties["Token"] = data;
                else
                    Application.Current.Properties.Add("Token", data);

            }
            catch (System.Exception)
            {
                set = false;
            }

            return set;

        }


    }
}