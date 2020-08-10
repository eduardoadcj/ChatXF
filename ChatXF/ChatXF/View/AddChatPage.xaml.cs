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
    public partial class AddChatPage : ContentPage {
        public AddChatPage() {
            InitializeComponent();
            BindingContext = new AddChatViewModel();
        }
    }
}