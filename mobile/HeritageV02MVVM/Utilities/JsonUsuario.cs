using HeritageV02MVVM.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace HeritageV02MVVM.Utilities
{
    public class JsonUsuario
    {

        public bool SetUsuarioJson(Usuario usuario)
        {

            bool set = true;

            try
            {

                string data = JsonConvert.SerializeObject(usuario);
                if (Application.Current.Properties.ContainsKey("Usuario"))
                    Application.Current.Properties["Usuario"] = data;
                else
                    Application.Current.Properties.Add("Usuario", data);

            }
            catch (System.Exception)
            {
                set = false;
            }

            return set;

        }

        public Usuario GetUsuarioJson()
        {

            Usuario usuario;

            try
            {

                if (Application.Current.Properties.ContainsKey("Usuario"))
                {
                    string data = Application.Current.Properties["Usuario"] as string;
                    Usuario usuario_json = JsonConvert.DeserializeObject<Usuario>(data);
                    usuario = usuario_json;
                }
                else
                    usuario = null;

            }
            catch (System.Exception)
            {
                usuario = null;
            }

            return usuario;
        }

    }
}
