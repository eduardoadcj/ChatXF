using ChatXF.Model;
using ChatXF.Service;
using ChatXF.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ChatXF.ViewModel {
    public class ChatListViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string nameProperty) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(nameProperty));
            }
        }

        private ChatService _Service;

        private Chat _SelectedChat;
        public Chat SelectedChat {
            get { return _SelectedChat; }
            set {
                _SelectedChat = value;
                OnPropertyChanged("SelectedChat");
                GoPaginaChat(value);
            }
        }

        private List<Chat> _ChatList;
        public List<Chat> ChatList { 
            get { return _ChatList; }
            set {
                _ChatList = value;
                OnPropertyChanged("ChatList");
            }
        }

        public Command AdicionarCommand { get; set; }
        public Command AtualizarCommand { get; set; }

        public ChatListViewModel() {
            AdicionarCommand = new Command(Adicionar);
            AtualizarCommand = new Command(() => { Task.Run(Atualizar); });
            _Service = new ChatService();
            Task.Run(Atualizar);
        }

        private void Adicionar() {
            ((NavigationPage) App.Current.MainPage).PushAsync(new AddChatPage());
        }

        private async Task Atualizar() {
            ChatList = await _Service.GetChats();
            ChatList = ChatList.OrderBy(a => a.nome).ToList();
        }

        private void GoPaginaChat(Chat chat) {
            if (chat == null)
                return;
            ((NavigationPage)App.Current.MainPage).Navigation.PushAsync(new ChatPage(chat));
            SelectedChat = null;
        }

    }
}
