using Prism.Mvvm;
using Newtonsoft.Json;

namespace HeritageV04.Utilities
{
    public class Repost : BindableBase
    {

        private string errorMessage;

        [JsonProperty("error")]
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        private string successMessage;

        [JsonProperty("success")]
        public string SuccessMessage
        {
            get { return successMessage; }
            set { SetProperty(ref successMessage, value); }
        }

        private int? id;

        [JsonProperty("id")]
        public int? Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private bool success;
        public bool Success
        {
            get { return success; }
            set 
            {
                if (SetProperty(ref success, value))
                {
                    Error = !Success;
                }
            }
        }

        private bool error;
        public bool Error
        {
            get { return error; }
            set
            {
                if (SetProperty(ref error, value))
                {
                    Success = !Error;
                }
            }
        }

    }
}
