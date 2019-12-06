using Newtonsoft.Json;

namespace HeritageV04.Models
{
    public class Historic : ModelBase
    {

        private string message;

        [JsonProperty("message")]
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        private string name;

        [JsonIgnore]
        public new string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

    }
}
