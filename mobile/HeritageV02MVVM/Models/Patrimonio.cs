﻿using Newtonsoft.Json;

namespace HeritageV02MVVM.Models
{
    public class Patrimonio : ModelBase
    {

        private string _nome_patrimonio;

        [JsonProperty("nome_patrimonio")]
        public string Nome_patrimonio
        {
            get => _nome_patrimonio;
            set => SetProperty(ref _nome_patrimonio, value);
        }

        private string _descricao;

        [JsonProperty("descricao")]
        public string Descricao
        {
            get => _descricao;
            set => SetProperty(ref _descricao, value);
        }

        private int? _id_ambiente;

        [JsonProperty("id_ambiente")]
        public int? Id_ambiente
        {
            get { return _id_ambiente; }
            set => SetProperty(ref _id_ambiente, value);
        }

        private string _codigo_patrimonio;

        [JsonProperty("codigo_patrimonio")]
        public string Codigo_patrimonio
        {
            get => _codigo_patrimonio; 
            set => SetProperty(ref _codigo_patrimonio, value);
        }

        private string _nome_ambiente;

        [JsonIgnore]
        public string Nome_ambiente
        {
            get => _nome_ambiente;
            set => SetProperty(ref _nome_ambiente, value);
        }
    }
}