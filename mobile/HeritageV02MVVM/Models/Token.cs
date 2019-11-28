using Prism.Mvvm;
using Newtonsoft.Json;

namespace HeritageV02MVVM.Models
{
    public class Token : BindableBase
    {

        private string _token_acesso;

        [JsonProperty("access_token")]
        public string Token_acesso
        {
            get => _token_acesso; 
            set => SetProperty(ref _token_acesso, value);
        }

    }

    public class Desc_Token : BindableBase
    {

        private string _token;

        [JsonProperty("token")]
        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

    }
}
