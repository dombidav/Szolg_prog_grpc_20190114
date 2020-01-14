using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for BookDataWindow.xaml
    /// </summary>
    public partial class BookDataWindow : Window
    {
        private readonly MainWindow mainWindow;
        private readonly BookModel baseModel;
        private readonly bool? isReadOnly;

        public BookDataWindow(MainWindow mainWindow, BookModel baseModel = null, bool? isReadOnly = null)
        {
            this.InitializeComponent();
            this.mainWindow = mainWindow;
            this.baseModel = baseModel;
            this.isReadOnly = isReadOnly;
            OkButton.IsEnabled = !isReadOnly ?? true;
            this.ResetButton_Click(null, null);
            this.KeyDown += this.BookDataWindow_KeyDown;
        }

        private void BookDataWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                this.OkButton_Click(null, null);
            else if(e.Key == Key.Delete && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                this.ResetButton_Click(null, null);
            else if ((e.Key == Key.Escape || e.Key == Key.W) && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                this.CancelButton_Click(null, null);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = new BookModel()
                {
                    Id = int.TryParse(inp_ID.Text, out var x) ? x : 0,
                    Title = inp_Title.Text,
                    Author = inp_Author.Text,
                    Publisher = inp_Publisher.Text,
                    Genre = inp_Genre.Text,
                    Isbn = inp_ISBN.Text,
                    PublishYear = inp_PublishYear.Text,
                    Description = inp_Description.Text,
                    Price = int.Parse(inp_Price.Text),
                    OnStorage = int.Parse(inp_OnStorage.Text),
                   // NotAvailable = chk_NotAvailable.IsChecked ?? false,
                    UserToken = MainWindow.userToken
                };
                if (isReadOnly == false)
                {
                    var ans = MainWindow.bookClient.EditBook(model);
                    if (ans.MessageType == AnswerModel.Types.MessageType.Ok)
                    {
                        _ = mainWindow.GetAndRefreshDbTable();
                        _ = MessageBox.Show(ans.Message, "Könyv szerkesztése", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        _ = MessageBox.Show(ans.Message, "Könyv szerkesztése", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (isReadOnly is null)
                {
                    var ans = MainWindow.bookClient.NewBook(model);
                    if (ans.MessageType == AnswerModel.Types.MessageType.Ok)
                    {
                        _ = mainWindow.GetAndRefreshDbTable();
                        _ = MessageBox.Show(ans.Message, "Új könyv", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        _ = MessageBox.Show(ans.Message, "Új könyv", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception){}
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            inp_ID.Text = baseModel?.Id.ToString() ?? "";
            inp_Title.Text = baseModel?.Title ?? "";
            inp_Author.Text = baseModel?.Author ?? "";
            inp_Publisher.Text = baseModel?.Publisher ?? "";
            inp_Genre.Text = baseModel?.Genre ?? "";
            inp_ISBN.Text = baseModel?.Isbn ?? "";
            inp_PublishYear.Text = baseModel?.PublishYear ?? "";
            inp_Price.Text = baseModel?.Price.ToString() ?? "";
            inp_OnStorage.Text = baseModel?.OnStorage.ToString() ?? "";
            inp_Description.Text = baseModel?.Description ?? "";
            //chk_NotAvailable.IsChecked = baseModel?.NotAvailable ?? false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isReadOnly ?? false)
            {
                inp_ID.IsReadOnly = true;
                inp_Title.IsReadOnly = true;
                inp_Author.IsReadOnly = true;
                inp_Publisher.IsReadOnly = true;
                inp_Genre.IsReadOnly = true;
                inp_ISBN.IsReadOnly = true;
                inp_PublishYear.IsReadOnly = true;
                inp_Price.IsReadOnly = true;
                inp_OnStorage.IsReadOnly = true;
                inp_Description.IsReadOnly = true;
                //chk_NotAvailable.IsEnabled = false;
            }
        }

        private void Inp_PublishYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
