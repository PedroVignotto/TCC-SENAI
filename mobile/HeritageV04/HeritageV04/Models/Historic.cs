using Newtonsoft.Json;
using System;

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

        private DateTime createdAt;

        [JsonProperty("createdAt")]
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { SetProperty(ref createdAt, value); }
        }
    }
}
