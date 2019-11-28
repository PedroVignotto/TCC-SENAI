using Newtonsoft.Json;

namespace HeritageV02MVVM.Models
{
    public class Historico : ModelBase
    {

        private int? _id_patrimonio;

        [JsonProperty("id_patrimonio")]
        public int? Id_patrimonio
        {
            get => _id_patrimonio;
            set => SetProperty(ref _id_patrimonio, value);
        }

        private int? _nome_historico;

        [JsonProperty("nome_historico")]
        public int? Nome_historico
        {
            get => _nome_historico;
            set => SetProperty(ref _nome_historico, value);
        }

        private int? _id_ambiente;

        [JsonProperty("id_ambiente")]
        public int? Id_ambiente
        {
            get => _id_ambiente;
            set => SetProperty(ref _id_ambiente, value);
        }

        private string _local_destino;

        [JsonProperty("local_destino")]
        public string Local_destino
        {
            get => _local_destino;
            set => SetProperty(ref _local_destino, value);
        }

        private string _criacao;

        [JsonProperty("created_at")]
        public string Criacao
        {
            get => _criacao;
            set => SetProperty(ref _criacao, value);
        }

        private string _atualizado;

        [JsonProperty("updated_at")]
        public string Atualizado
        {
            get => _atualizado;
            set => SetProperty(ref _atualizado, value);
        }

        private string _descricao;

        [JsonProperty("descricao")]
        public string Descricao
        {
            get => _descricao;
            set => SetProperty(ref _descricao, value);
        }

        public class Manutencao : ModelBase
        {
           
            private string _nome;

            [JsonIgnore]
            public string Nome
            {
                get => _nome;
                set => SetProperty(ref _nome, value);
            }

            private int? _id_patrimonio;

            [JsonProperty("id_patrimonio")]
            public int? Id_patrimonio
            {
                get => _id_patrimonio;
                set => SetProperty(ref _id_patrimonio, value);
            }

            private int? _id_ambiente;

            [JsonProperty("id_ambiente")]
            public int? Id_ambiente
            {
                get => _id_ambiente;
                set => SetProperty(ref _id_ambiente, value);
            }

            private string _codigo_patrimonio;

            [JsonProperty("codigo_patrimonio")]
            public string Codigo_patrimonio
            {
                get => _codigo_patrimonio;
                set => SetProperty(ref _codigo_patrimonio, value);
            }

            private string _descricao;

            [JsonProperty("descricao")]
            public string Descricao
            {
                get => _descricao;
                set => SetProperty(ref _descricao, value);
            }

            private string _criacao;

            [JsonProperty("created_at")]
            public string Criacao
            {
                get => _criacao;
                set => SetProperty(ref _criacao, value);
            }

        }

        public class Movimentacao : ModelBase
        {

            private string _nome;

            [JsonIgnore]
            public string Nome
            {
                get => _nome;
                set => SetProperty(ref _nome, value);
            }

            private int? _id_patrimonio;

            [JsonProperty("id_patrimonio")]
            public int? Id_patrimonio
            {
                get => _id_patrimonio;
                set => SetProperty(ref _id_patrimonio, value);
            }

            private int? _id_ambiente;

            [JsonProperty("id_ambiente")]
            public int? Id_ambiente
            {
                get => _id_ambiente;
                set => SetProperty(ref _id_ambiente, value);
            }

            private string _local_destino;

            [JsonProperty("local_destino")]
            public string Local_destino
            {
                get => _local_destino;
                set => SetProperty(ref _local_destino, value);
            }

            private string _nome_ambiente;

            [JsonIgnore]
            public string Nome_ambiente
            {
                get => _nome_ambiente;
                set => SetProperty(ref _nome_ambiente, value);
            }

            private string _codigo_patrimonio;

            [JsonProperty("codigo_patrimonio")]
            public string Codigo_patrimonio
            {
                get => _codigo_patrimonio;
                set => SetProperty(ref _codigo_patrimonio, value);
            }

            private string _criacao;

            [JsonProperty("created_at")]
            public string Criacao
            {
                get => _criacao;
                set => SetProperty(ref _criacao, value);
            }

        }
    }
}
