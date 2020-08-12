using ChatXF.Model;
using ChatXF.Service;
using ChatXF.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace ChatXF.ViewModel {
    public class ChatViewModel : INotifyPropertyChanged {
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private UserSessionManager _UserSession;
        private ChatService _Service;
        
        private Chat _CurrentChat;
        private Chat CurrentChat {
            get { return _CurrentChat; }
            set {
                _CurrentChat = value;
                OnPropertyChanged("CurrentChat");
            }
        }

        private List<Mensagem> _Mensagens;
        public List<Mensagem> Mensagens {
            get { return _Mensagens; }
            set {
                _Mensagens = value;
                OnPropertyChanged("Mensagens");
            }
        }

        private string _Mensagem;
        public string Mensagem {
            get { return _Mensagem; }
            set {
                _Mensagem = value;
                OnPropertyChanged("Mensagem");
            }
        }

        public Command EnviarCommand { get; set; }
        public Command AtualizarCommand { get; set; }
        
        public ChatViewModel(Chat chat) {
            EnviarCommand = new Command(Enviar);
            AtualizarCommand = new Command(UpdateChat);
            _UserSession = new UserSessionManager();
            _Service = new ChatService();
            CurrentChat = chat;
            UpdateChat();
        }

        private void UpdateChat() {
            Mensagens = _Service.GetMensagensChat(CurrentChat);
        }

        private void Enviar() {
            if (Mensagem == null || Mensagem.Length == 0)
                return;
            var user = _UserSession.GetUsuario();
            var msg = new Mensagem() {
                id_usuario = user.id,
                mensagem = Mensagem,
                id_chat = CurrentChat.id
            };
            if(_Service.InsertMensagem(msg))
                Mensagem = "";
            UpdateChat();
        }

    }
}
