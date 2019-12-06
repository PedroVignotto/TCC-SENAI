using Newtonsoft.Json;

namespace HeritageV04.Models
{
    public class Environment : ModelBase
    {

        private int? userId;

        [JsonProperty("user_id")]
        public int? UserId
        {
            get { return userId; }
            set { SetProperty(ref userId, value); }
        }

        private string environmentName;

        [JsonIgnore]
        public string EnvironmentName
        {
            get { return environmentName; }
            set { SetProperty(ref environmentName, value); }
        }

        private string userName;

        [JsonIgnore]
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        private int quantityHeritages;

        [JsonIgnore]
        public int QuantityHeritages
        {
            get { return quantityHeritages; }
            set { SetProperty(ref quantityHeritages, value); }
        }

    }
}
