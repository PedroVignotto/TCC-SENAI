using HeritageV02MVVM.Models;
using Xamarin.Forms;

namespace HeritageV02MVVM.Utilities
{
    public class RequestToken
    {
        public Token Token;

        public RequestToken()
        {
            if (Application.Current.Properties.ContainsKey("Token"))
                Token = new Token() { Token_acesso = Application.Current.Properties["Token"] as string, };
            else
                Token = null;
        }

        public void SetToken(string token)
        {
            if (Application.Current.Properties.ContainsKey("Token"))
                Application.Current.Properties["Token"] = token;
            else
                Application.Current.Properties.Add("Token", token);
        }
    }
}
