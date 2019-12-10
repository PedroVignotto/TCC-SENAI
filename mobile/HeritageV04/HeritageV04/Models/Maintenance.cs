using Prism.Mvvm;
using Newtonsoft.Json;

namespace HeritageV04.Models
{
    public class Maintenance : BindableBase
    {

        private int? heritageId;

        [JsonProperty("id")]
        public int? HeritageId
        {
            get { return heritageId; }
            set { SetProperty(ref heritageId, value); }
        }

        private string code;

        [JsonProperty("code")]
        public string Code
        {
            get { return code; }
            set { SetProperty(ref code, value); }
        }

        private string problem;

        [JsonProperty("problem")]
        public string Problem
        {
            get { return problem; }
            set { SetProperty(ref problem, value); }
        }

        private int? companyId;

        [JsonProperty("company_id")]
        public int? CompanyId
        {
            get { return companyId; }
            set { SetProperty(ref companyId, value); }
        }


    }
}
