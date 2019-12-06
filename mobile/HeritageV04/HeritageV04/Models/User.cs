using Newtonsoft.Json;
using Prism.Mvvm;

namespace HeritageV04.Models
{
    public class User : ModelBase
    {

        private string password;

        [JsonProperty("password")]
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        private string email;

        [JsonProperty("email")]
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        private int? avatarId;

        [JsonProperty("avatar_id")]
        public int? AvatarId
        {
            get { return avatarId; }
            set { SetProperty(ref avatarId, value); }
        }

        private Avatar avatar;

        [JsonProperty("avatar")]
        public Avatar Avatar
        {
            get { return avatar; }
            set { SetProperty(ref avatar, value); }
        }

    }

    public class Avatar : BindableBase
    {

        private string _url;

        [JsonProperty("url")]
        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        private int? _id;

        [JsonProperty("id")]
        public int? Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _path;

        [JsonProperty("path")]
        public string Path
        {
            get { return _path; }
            set { SetProperty(ref _path, value); }
        }

    }

    public class UserRoot : BindableBase
    {

        private User user;

        [JsonProperty("user")]
        public User User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        private string token;

        [JsonProperty("token")]
        public string Token
        {
            get { return token; }
            set { SetProperty(ref token, value); }
        }

    }
}
