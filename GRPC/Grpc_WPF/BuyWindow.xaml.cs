using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SzolgProg_vizsga;

namespace Grpc_WPF
{
    /// <summary>
    /// Interaction logic for BuyWindow.xaml
    /// </summary>
    public partial class BuyWindow : Window
    {
        private readonly MainWindow mainWindow;
        private readonly BookModel selectedItem;

        public BuyWindow(MainWindow mainWindow, SzolgProg_vizsga.BookModel selectedItem)
        {
            this.InitializeComponent();
            this.mainWindow = mainWindow;
            this.selectedItem = selectedItem;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(inp_number.Text, out var x))
                mainWindow.BuyBooks(selectedItem.Id, x);
        }

        private void Inp_bookName_Loaded(object sender, RoutedEventArgs e) => inp_bookName.Text = $"{selectedItem.Author}: {selectedItem.Title} ({selectedItem.PublishYear})";
    }
}
