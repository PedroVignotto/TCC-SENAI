using Newtonsoft.Json;

namespace HeritageV04.Models
{
    public class Heritage : ModelBase
    {

        private string description;

        [JsonProperty("description")]
        [JsonRequired]
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        private string code;

        [JsonProperty("code")]
        [JsonRequired]
        public string Code
        {
            get { return code; }
            set { SetProperty(ref code, value); }
        }

        private bool state;

        [JsonProperty("state")]
        public bool State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }

        private string environmentName;

        [JsonIgnore]
        public string EnvironmentName
        {
            get => environmentName;
            set => SetProperty(ref environmentName, value);
        }

        private string messageState;

        [JsonIgnore]
        public string MessageState
        {
            get => messageState;
            set => SetProperty(ref messageState, value);
        }

    }
}
