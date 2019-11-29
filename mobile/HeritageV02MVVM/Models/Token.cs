using Prism.Mvvm;
using Newtonsoft.Json;
using System;

namespace HeritageV02MVVM.Models
{
    public class Token : BindableBase
    {

        public Token()
        {
            Hora_Registro = DateTime.Now;
        }

        private string _token_acesso;

        [JsonProperty("access_token")]
        public string Token_acesso
        {
            get => _token_acesso; 
            set => SetProperty(ref _token_acesso, value);
        }

        private int _expira_em;

        [JsonProperty("expires_in")]
        public int Expira_em
        {
            get => _expira_em; 
            set => SetProperty(ref _expira_em, value);
        }

        private DateTime _hora_registro;

        public DateTime Hora_Registro
        {
            get => _hora_registro;
            set => SetProperty(ref _hora_registro, value);
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
