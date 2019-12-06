using HeritageV04.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HeritageV04.Utilities
{
    public class UserJson
    {

        public bool SetUsuarioJson(User user)
        {

            bool set = true;

            try
            {

                string data = JsonConvert.SerializeObject(user);
                if (Application.Current.Properties.ContainsKey("User"))
                    Application.Current.Properties["User"] = data;
                else
                    Application.Current.Properties.Add("User", data);

            }
            catch (System.Exception)
            {
                set = false;
            }

            return set;

        }

        public User GetUsuarioJson()
        {

            User user;

            try
            {

                if (Application.Current.Properties.ContainsKey("Usuario"))
                {
                    string data = Application.Current.Properties["Usuario"] as string;
                    User userJson = JsonConvert.DeserializeObject<User>(data);
                    user = userJson;
                }
                else
                    user = null;

            }
            catch (System.Exception)
            {
                user = null;
            }

            return user;
        }

    }
}
