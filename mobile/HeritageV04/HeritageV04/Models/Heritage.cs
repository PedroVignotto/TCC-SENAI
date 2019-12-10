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

        private string messageState;

        [JsonIgnore]
        public string MessageState
        {
            get => messageState;
            set => SetProperty(ref messageState, value);
        }

        private int? environmentId;

        [JsonProperty("environment_id")]
        public int? EnvironmentId
        {
            get { return environmentId; }
            set { SetProperty(ref environmentId, value); }
        }

        private Environment environment;

        [JsonProperty("environment")]
        public Environment Environment
        {
            get { return environment; }
            set { SetProperty(ref environment, value); }
        }

        private bool serializeCode;

        [JsonIgnore]
        public bool SerializeCode
        {
            get { return serializeCode; }
            set { SetProperty(ref serializeCode, value); }
        }

        private bool serializeEnvironmentId;

        [JsonIgnore]
        public bool SerializeEnvironmentId
        {
            get { return serializeEnvironmentId; }
            set { SetProperty(ref serializeEnvironmentId, value); }
        }

        public bool ShouldSerializeCode()
        {
            return SerializeCode;
        }

        public bool ShouldSerializeEnvironmentId()
        {
            return serializeEnvironmentId;
        }
    }
}
