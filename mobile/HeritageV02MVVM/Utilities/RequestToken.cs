using HeritageV02MVVM.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HeritageV02MVVM.Utilities
{
    public class RequestToken
    {
        public Token GetToken()
        {
            Token token;

            if (Application.Current.Properties.ContainsKey("Token"))
            {
                string data = Application.Current.Properties["Token"] as string;
                token = JsonConvert.DeserializeObject<Token>(data);
            }
            else
                token = null;

            return token;
        }

        public bool SetToken(Token token)
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
