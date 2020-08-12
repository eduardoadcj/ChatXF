using ChatXF.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ChatXF.Util {
    public class MensagemDataTemplateSelector : DataTemplateSelector {

        private Usuario usuarioLogado;

        public MensagemDataTemplateSelector() : base() {
            usuarioLogado = new UserSessionManager().GetUsuario();
        }

        public DataTemplate MensagemUsuarioTemplate { get; set; }
        public DataTemplate MensagemTemplate { get; set; }
        
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container) {
            var mensagem = item as Mensagem;
            return mensagem.id_usuario == usuarioLogado.id ? MensagemUsuarioTemplate : MensagemTemplate;
        }

    }
}
