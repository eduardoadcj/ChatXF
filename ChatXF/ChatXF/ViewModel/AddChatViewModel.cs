using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using ChatXF.Model;
using ChatXF.Service;
using ChatXF.View;

namespace ChatXF.ViewModel {
    public class AddChatViewModel : INotifyPropertyChanged {
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Nome { get; set; }

        private string _Error;
        public string Error {
            get { return _Error; }
            set {
                _Error = value;
                OnPropertyChanged("Error");
            }
        }

        public Command SalvarCommand { get; set; }

        public AddChatViewModel() {
            SalvarCommand = new Command(AttemptSalvar);
        }

        private void AttemptSalvar() {
            if(Nome == null || Nome.Length == 0) {
                Error = "Preencha o campo nome.";
                return;
            }
            Salvar();
        }

        private void Salvar() {
            var chat = new Chat() {
                nome = Nome
            };
            bool ok = new ChatService().InsertChat(chat);
            if (ok) {
                GoBackAndUpdate();
            } else {
                Error = "Erro ao cadastrar o chat. Tente novamente mais tarde";
            }
        }

        private void GoBackAndUpdate() {
            ((NavigationPage)App.Current.MainPage).PopAsync();
            var nav = (NavigationPage)App.Current.MainPage;
            var chatsViewModel = ((ChatListPage)nav.CurrentPage).BindingContext as ChatListViewModel;
            if (chatsViewModel.AtualizarCommand.CanExecute(null))
                chatsViewModel.AtualizarCommand.Execute(null);
        }

    }
}
