using Newtonsoft.Json;

namespace HeritageV04.Models
{
    public class File : ModelBase
    {

        private string path;

        [JsonProperty("path")]
        public string Path
        {
            get { return path; }
            set { SetProperty(ref path, value); }
        }

    }
}
