using ChatXF.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatXF.Util {
    public class UserSessionManager {

        public Usuario GetUsuario() {
            if (!App.Current.Properties.ContainsKey("LOGIN"))
                return null;
            var user = App.Current.Properties["LOGIN"] as string;
            return JsonConvert.DeserializeObject<Usuario>(user);
        }

        public void SetUsuario(Usuario usuario) {
            App.Current.Properties["LOGIN"] = JsonConvert.SerializeObject(usuario);
        }

    }
}
