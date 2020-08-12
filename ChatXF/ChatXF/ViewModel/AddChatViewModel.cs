using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using ChatXF.Model;
using ChatXF.Service;
using ChatXF.View;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ChatXF.ViewModel {
    public class AddChatViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Nome { get; set; }

        private bool _Enviando;
        public bool Enviando {
            get { return _Enviando; }
            set {
                _Enviando = value;
                OnPropertyChanged("Enviando");
            }
        }

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
            if (Nome == null || Nome.Length == 0) {
                Error = "Preencha o campo nome.";
                return;
            }
            Task.Run(Salvar);
        }

        private async Task Salvar() {
            var chat = new Chat() {
                nome = Nome
            };
            Enviando = true;
            bool ok = await new ChatService().InsertChat(chat);
            if (ok) {
                //Comandos de navegação só funcionam na thread principal
                Device.BeginInvokeOnMainThread(GoBackAndUpdate);
            } else {
                Error = "Erro ao cadastrar o chat. Tente novamente mais tarde";
            }
            Enviando = false;
        }

        private void GoBackAndUpdate() {
            var currentPage = ((NavigationPage)App.Current.MainPage);
            var chatsViewModel = ((ChatListPage)currentPage.RootPage).BindingContext as ChatListViewModel;
            if (chatsViewModel.AtualizarCommand.CanExecute(null))
                chatsViewModel.AtualizarCommand.Execute(null);
            currentPage.PopAsync();
        }

    }
}
