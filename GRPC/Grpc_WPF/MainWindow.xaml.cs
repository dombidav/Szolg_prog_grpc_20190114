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
using ClientLib;
using Grpc.Core;
using SzolgProg_vizsga;

namespace Grpc_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Book.BookClient bookClient;
        public static string userToken = "";
        public static string userStatusText = "";

        public MainWindow()
        {
            this.InitializeComponent();
            GrpcInit();
        }

        private static void GrpcInit() => bookClient = GrpcLib.BookServiceInit;

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(userToken))
            {
                var loginWindow = new Login(this);
                loginWindow.Show();
                _ = loginWindow.Activate();
            }
            else
            {
                var ans = bookClient.Logout(new UserModel() { Token = userToken });
                if(ans.MessageType == AnswerModel.Types.MessageType.Ok)
                {
                    userStatusText = "Kijelentkezve";
                    userToken = "";
                    LoginButton.Content = "Bejelentkezés";
                }
                _ = MessageBox.Show(ans.Message, "Kijelentkezés", MessageBoxButton.OK, ans.MessageType == AnswerModel.Types.MessageType.Ok ? MessageBoxImage.Information : MessageBoxImage.Error);
            }
        }

        internal void BuyBooks(int id, int x)
        {
            var ans = bookClient.BuyBook(new BookLookupModel() { Id = id, UserToken = userToken, Number = x });
            _ = MessageBox.Show(ans.Message, "Vásárlás", MessageBoxButton.OK, ans.MessageType == AnswerModel.Types.MessageType.Ok ? MessageBoxImage.Information : MessageBoxImage.Error);
            this.TextBox_BookTitle_TextChanged(null, null);
        }

        public void LoggedIn(string statusText, string token)
        {
            userStatusText = statusText;
            userToken = token;
            LoginButton.Content = "Kijelentkezés";
            _ = this.Activate();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if(datagrid_main.SelectedItems.Count > 0)
            {
                var bookWindow = new BookDataWindow(this, (BookModel)datagrid_main.SelectedItem, false);
                bookWindow.Show();
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            var bookWindow = new BookDataWindow(this);
            bookWindow.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_main.SelectedItems.Count > 0)
            {
                var msgBoxAns = MessageBox.Show("Biztosan törlöd ezt az elemet? Ez a művelet nem visszavonható!", "Könyv törlése", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (msgBoxAns == MessageBoxResult.Yes)
                {
                    var ans = bookClient.DeleteBook(new BookLookupModel() { Id = ((BookModel)datagrid_main.SelectedItem).Id, UserToken = userToken });
                    _ = MessageBox.Show(ans.Message, "Könyv törlése", MessageBoxButton.OK, ans.MessageType == AnswerModel.Types.MessageType.Ok ? MessageBoxImage.Information : MessageBoxImage.Error);
                    _ = this.GetAndRefreshDbTable(); //Nem probléma, ha nem várja meg amíg elkészül
                    _ = this.Activate();
                } 
            }
        }

        private async void Datagrid_main_Loaded(object sender, RoutedEventArgs e)
        {
            var col1 = new DataGridTextColumn();
            var col2 = new DataGridTextColumn();
            var col3 = new DataGridTextColumn();
            var col4 = new DataGridTextColumn();
            var col5 = new DataGridTextColumn();
            var col6 = new DataGridTextColumn();
            var col7 = new DataGridTextColumn();
            var col8 = new DataGridTextColumn();
            var col9 = new DataGridTextColumn();
            var col10 = new DataGridTextColumn();
            //var col11 = new DataGridCheckBoxColumn();
            datagrid_main.Columns.Add(col1);
            datagrid_main.Columns.Add(col2);
            datagrid_main.Columns.Add(col3);
            datagrid_main.Columns.Add(col4);
            datagrid_main.Columns.Add(col5);
            datagrid_main.Columns.Add(col6);
            datagrid_main.Columns.Add(col8);
            datagrid_main.Columns.Add(col9);
            datagrid_main.Columns.Add(col10);
            //datagrid_main.Columns.Add(col11);
            datagrid_main.Columns.Add(col7);
            col1.Binding = new Binding("Id");
            col2.Binding = new Binding("Title");
            col3.Binding = new Binding("Author");
            col4.Binding = new Binding("Publisher");
            col5.Binding = new Binding("Isbn");
            col6.Binding = new Binding("Genre");
            col7.Binding = new Binding("Description");
            col8.Binding = new Binding("PublishYear");
            col9.Binding = new Binding("Price");
            col10.Binding = new Binding("OnStorage");
            //col11.Binding = new Binding("NotAvailable");
            col1.Header = "ID";
            col2.Header = "Cím";
            col3.Header = "Író";
            col4.Header = "Kiadó";
            col5.Header = "ISBN";
            col6.Header = "Műfaj";
            col7.Header = "Leírás";
            col8.Header = "Kiadás éve";
            col9.Header = "Ár";
            col10.Header = "Készleten";
            //col11.Header = "Nem elérhető";
            await this.GetAndRefreshDbTable();
        }

        public async Task GetAndRefreshDbTable()
        {
            datagrid_main.Items.Clear();
            if (string.IsNullOrWhiteSpace(TextBox_BookTitle.Text))
                TextBox_BookTitle.Text = ""; //Ne legyen null érték benne
            try
            {
                using (var call = bookClient.GetBooksByTitle(new BookSearchModel() { Name = TextBox_BookTitle.Text }))
                {
                    while (await call.ResponseStream.MoveNext())
                    {
                        var current = call.ResponseStream.Current;
                        _ = datagrid_main.Items.Add(current);//TODO
                    }
                }

            }
            catch (Exception)
            {
            }
        }

        private void Datagrid_main_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (datagrid_main.SelectedItems.Count > 0)
            {
                var bookWindow = new BookDataWindow(this, (BookModel)datagrid_main.SelectedItem, string.IsNullOrWhiteSpace(userToken));
                bookWindow.Show();
            } 
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F12 && !(datagrid_main.SelectedItem is null))
            {
                var buyWindow = new BuyWindow(this, (BookModel)datagrid_main.SelectedItem);
                buyWindow.Show();
            }
        }

        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid_main.SelectedItems.Count > 0)
            {
                var buyWindow = new BuyWindow(this, (BookModel)datagrid_main.SelectedItem);
                buyWindow.Show(); 
            }
        }

        private void TextBox_BookTitle_TextChanged(object sender, TextChangedEventArgs e) => _ = this.GetAndRefreshDbTable(); //Nincs más parancs, nem kell "await", ha ott lenne, akkor az egésznek async-nek kéne lennie, de akkor a wpf nem fogadná el event target-nek
    }
}
