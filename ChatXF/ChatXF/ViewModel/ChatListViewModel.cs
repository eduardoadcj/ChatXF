using ChatXF.Model;
using ChatXF.Service;
using ChatXF.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using System.Linq;

namespace ChatXF.ViewModel {
    public class ChatListViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string nameProperty) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(nameProperty));
            }
        }

        private ChatService _Service;

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
            AtualizarCommand = new Command(Atualizar);
            _Service = new ChatService();
            Atualizar();
        }

        private void Adicionar() {
            ((NavigationPage) App.Current.MainPage).PushAsync(new AddChatPage());
        }

        private void Atualizar() {
            ChatList = _Service.GetChats();
            ChatList = ChatList.OrderBy(a => a.nome).ToList();
        }

    }
}
