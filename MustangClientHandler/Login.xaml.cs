using MustangClientHandler.EF;
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
using System.Windows.Shapes;

namespace MustangClientHandler
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            bool legal = await LoginAsync(this.textBoxLogin.Text, PrepatePassword(this.passwordBox.Password));
            if (!legal)
            {
                this.textBlockHeading.Text = "Wrong login or password";
                this.textBlockHeading.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }
            MainWindow main = new MainWindow(true);
            Application.Current.MainWindow = main;
            this.Close();
            main.Show();
        }
        private async Task<bool> LoginAsync(string login, string pass)
        {
            var temp = await Task.Run(() => LoginSync(login, pass));
            return temp;
        }
        private bool LoginSync(string login, string pass)
        {
            msContext _context = new msContext();
            msUser _user = _context.msUsers.FirstOrDefault(user => user.UserLogin == login && user.UserPassword == pass);
            if (_user == null)
                return false;
            msUser.CurrentUser = _user;
            return true;
        }
        private string PrepatePassword(string arg)
        {
            //some hashing here
            return arg;
        }
    }
}
