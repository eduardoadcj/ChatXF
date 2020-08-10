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

        private StackLayout _MessageContainer;
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
                ShowOnScreen();
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
        
        public ChatViewModel(Chat chat, StackLayout messageContainer) {
            EnviarCommand = new Command(Enviar);
            _UserSession = new UserSessionManager();
            _Service = new ChatService();
            _MessageContainer = messageContainer;
            CurrentChat = chat;
            UpdateChat();
        }

        private void UpdateChat() {
            Mensagens = _Service.GetMensagensChat(CurrentChat);
        }

        private void ShowOnScreen() {
            _MessageContainer.Children.Clear();

            if (Mensagens == null)
                return;

            var user = _UserSession.GetUsuario();
            foreach (var msg in Mensagens) {
                if(msg.usuario.id == user.id) {
                    _MessageContainer.Children.Add(CreateUserMessage(msg));
                } else {
                    _MessageContainer.Children.Add(CreateMessage(msg));
                }
            }

        }

        private Xamarin.Forms.View CreateMessage(Mensagem msg) {
            var frame = new Frame() {
                OutlineColor = (Color)App.Current.Resources["primaryDarkColor"],
                HorizontalOptions = LayoutOptions.Start
            };
            var layout = new StackLayout();
            var userLabel = new Label() {
                FontSize = 10,
                TextColor = (Color)App.Current.Resources["primaryDarkColor"],
                Text = msg.usuario.nome
            };
            var mensagemLabel = new Label() {
                TextColor = (Color)App.Current.Resources["primaryDarkColor"],
                Text = msg.mensagem
            };

            layout.Children.Add(userLabel);
            layout.Children.Add(mensagemLabel);
            frame.Content = layout;

            return frame;
        }

        private Xamarin.Forms.View CreateUserMessage(Mensagem msg) {
            var layout = new StackLayout() {
                Padding = 15,
                BackgroundColor = (Color)App.Current.Resources["primaryColor"],
                HorizontalOptions = LayoutOptions.End
            };
            var label = new Label() {
                TextColor = Color.White,
                Text = msg.mensagem
            };
            layout.Children.Add(label);
            return layout;
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
