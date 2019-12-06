using Prism.Mvvm;
using Newtonsoft.Json;

namespace HeritageV04.Models
{
    public class ModelBase : BindableBase
    {

        private int? companyId;

        [JsonProperty("company_id")]
        public int? CompanyId
        {
            get { return companyId; }
            set { SetProperty(ref companyId, value); }
        }

        private int? id;

        [JsonProperty("id")]
        public int? Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string name;

        [JsonProperty("name")]
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

    }
}
