using Newtonsoft.Json;
using Prism.Mvvm;

namespace HeritageV02MVVM.Models
{
    public class ModelBase : BindableBase
    {

        private int? _id;

        [JsonProperty("id")]
        public int? Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int? _id_empresa;

        [JsonProperty("id_empresa")]
        public int? Id_empresa
        {
            get => _id_empresa;
            set => SetProperty(ref _id_empresa, value);
        }

    }
}
