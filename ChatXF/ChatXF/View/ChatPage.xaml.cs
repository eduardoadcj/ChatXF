using ChatXF.Model;
using ChatXF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatXF.View {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage {
        public ChatPage(Chat chat) {
            InitializeComponent();
            Title = chat.nome;
            BindingContext = new ChatViewModel(chat);
        }
    }
}