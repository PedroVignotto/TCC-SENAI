using Newtonsoft.Json;
using Prism.Mvvm;

namespace HeritageV02MVVM.Models
{
    public class Ambiente : ModelBase
    {

        private string _nome_ambiente;

        [JsonProperty("nome_ambiente")]
        public string Nome_ambiente
        {
            get => _nome_ambiente;
            set => SetProperty(ref _nome_ambiente, value);
        }

        private int? _id_usuario;

        [JsonProperty("id_usuario")]
        public int? Id_usuario
        {
            get => _id_usuario;
            set => SetProperty(ref _id_usuario, value);
        }

        private int _quantidade_patrimonio;

        [JsonIgnore]
        public int Quantidade_patrimonio
        {
            get => _quantidade_patrimonio;
            set => SetProperty(ref _quantidade_patrimonio, value);
        }

        private string _nome_usuario;

        [JsonIgnore]
        public string Nome_usuario
        {
            get => _nome_usuario; 
            set => SetProperty(ref _nome_usuario, value);
        }

        public class Ambiente_Update : ModelBase
        {

            private string _nome_ambiente;

            [JsonProperty("nome_ambiente")]
            public string Nome_ambiente
            {
                get => _nome_ambiente;
                set => SetProperty(ref _nome_ambiente, value);
            }

        }

    }
}
