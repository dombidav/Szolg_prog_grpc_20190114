﻿using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SzolgProg_vizsga;

namespace gRPC_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GrpcChannel channel;
        private Book.BookClient bookClient;

        public MainWindow() => this.InitializeComponent();

        private void Login_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void New_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e) =>  this.InitAsync();

        private async Task InitAsync()
        {
            Settings.Load();
            channel = GrpcChannel.ForAddress(address: $"{Settings.ChannelProtocol}://{Settings.ChannelHost}:{Settings.ChannelPort}");
            bookClient = new Book.BookClient(channel);
            var res = await bookClient.GetBooksByIdAsync(new BookLookupModel() { Id = 1 });
            _ = MessageBox.Show(res.Description);
        }
    }
}
