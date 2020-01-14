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

namespace Grpc_WPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        readonly MainWindow mainWindow;
        public Login(MainWindow mainWindow)
        {
            this.InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(inp_token.Text))
            {
                var ans = MainWindow.bookClient.Login(new SzolgProg_vizsga.UserModel() { Token = inp_token.Text });
                if(ans.MessageType == SzolgProg_vizsga.AnswerModel.Types.MessageType.Ok)
                {
                    mainWindow.LoggedIn(ans.Message, inp_token.Text);
                }
                _ = MessageBox.Show(ans.Message, "Bejelentkezés", MessageBoxButton.OK, ans.MessageType == SzolgProg_vizsga.AnswerModel.Types.MessageType.Ok ? MessageBoxImage.Information : MessageBoxImage.Error);
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Inp_token_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.Ok_Click(null, null);
            else if (e.Key == Key.Escape)
                this.Cancel_Click(null, null);
        }

        private void Window_Activated(object sender, EventArgs e) => inp_token.Focus();
    }
}
