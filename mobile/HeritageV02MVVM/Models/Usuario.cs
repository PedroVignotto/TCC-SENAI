using Newtonsoft.Json;

namespace HeritageV02MVVM.Models
{
    public class Usuario : ModelBase
    {

        private string _name;

        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _nivel_usuario;

        [JsonIgnore]
        public string Nivel_usuario
        {
            get => _nivel_usuario;
            set => SetProperty(ref _nivel_usuario, value);
        }

        private string _email;

        [JsonProperty("email")]
        public string Email
        {
            get => _email; 
            set => SetProperty(ref _email, value);
        }

        private int? _id_nivel_usuario;

        [JsonProperty("id_nivel_usuario")]
        public int? Id_nivel_usuario
        {
            get => _id_nivel_usuario;
            set => SetProperty(ref _id_nivel_usuario, value);
        }

        private string _imagem;

        [JsonProperty("imagem")]
        public string Imagem
        {
            get => _imagem;
            set => SetProperty(ref _imagem, value);
        }
        
        private string _token;

        [JsonIgnore]
        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        private string _password;

        [JsonProperty("password")]
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public class Usuario_Update : ModelBase
        {

            private string _name;

            [JsonProperty("name")]
            public string Name
            {
                get => _name;
                set => SetProperty(ref _name, value);
            }

            private string _email;

            [JsonProperty("email")]
            public string Email
            {
                get => _email;
                set => SetProperty(ref _email, value);
            }

            private int? _id_nivel_usuario;

            [JsonProperty("id_nivel_usuario")]
            public int? Id_nivel_usuario
            {
                get => _id_nivel_usuario;
                set => SetProperty(ref _id_nivel_usuario, value);
            }

            private string _imagem;

            [JsonProperty("imagem")]
            public string Imagem
            {
                get => _imagem;
                set => SetProperty(ref _imagem, value);
            }

        }

    }
}
