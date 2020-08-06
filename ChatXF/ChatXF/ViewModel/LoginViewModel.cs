using ChatXF.Model;
using ChatXF.Service;
using ChatXF.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace ChatXF.ViewModel {
    public class LoginViewModel : INotifyPropertyChanged {

        private ChatService _Service;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange(string nameProperty) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(nameProperty));
            }
        }

        private string _Nome;
        public string Nome { 
            get { return _Nome; }
            set {
                _Nome = value;
                OnPropertyChange("Nome");
            }
        }

        private string _Senha;
        public string Senha {
            get { return _Senha; }
            set {
                _Senha = value;
                OnPropertyChange("Senha");
            }
        }

        private string _Error;
        public string Error {
            get { return _Error; }
            set {
                _Error = value;
                OnPropertyChange("Error");
            }
        }

        public Command EntrarCommand { get; set; }

        public LoginViewModel() {
            _Service = new ChatService();
            EntrarCommand = new Command(AttemptEntrar);
        }

        private void AttemptEntrar() {
            if(Nome == null || Nome.Length == 0 || Senha == null || Senha.Length == 0) {
                Error = "Preencha todos os campos.";
                return;
            }
            var usu = new Usuario();
            usu.nome = Nome;
            usu.password = Senha;
            Entrar(usu);
        }

        private void Entrar(Usuario usuario) {
            var usu = _Service.GetUsuario(usuario);
            if(usu == null) {
                Error = "Usuário não encontrado.";
                return;
            }
            App.Current.Properties["LOGIN"] = JsonConvert.SerializeObject(usu);
            App.Current.MainPage = new NavigationPage(new ChatListPage());
        }

    }
}
