﻿using ChatXF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatXF.View {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatListPage : ContentPage {
        public ChatListPage() {
            InitializeComponent();
            BindingContext = new ChatListViewModel();
        }
    }
}