using MustangClientHandler.EF;
using MustangClientHandler.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace MustangClientHandler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool UserInAdminRole { get; }
        public const string _defaultSearchText = "Enter client name";
        public const string _defWindowTitle = "Main Window";
        public MainWindow()
        {
            InitializeComponent();

        }
        public MainWindow(bool userInAdminRole):this()
        {
            this.UserInAdminRole = userInAdminRole;
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void SearchText_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)=> UpdateActions.onFocus(this);

        private void SearchText_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)=> UpdateActions.focusLost(this);

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)=> UpdateActions.textChanged(this);

        private void Window_Loaded(object sender, RoutedEventArgs e)=> UpdateActions.setButtons(this, false);

        private void ListBox_GotMouseCapture(object sender, MouseEventArgs e)=> UpdateActions.setButtons(this, true);

        private void ListBox_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void ListBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if(this.ListBox.SelectedItem==null)
            UpdateActions.setButtons(this, false);
        }

        private void GetPayment_Click(object sender, RoutedEventArgs e)
        {
            if (this.ListBox.SelectedItem == null)
            {
                this.GetPayment.IsEnabled = false;
                return;
            }
            string num = this.ListBox.SelectedItem.ToString().Substring(3).Split(',')[0];
            ChangeWindow(new GetPayment(int.Parse(num)));
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)=> ChangeWindow(new AddClient());
        private void ChangeWindow<T>(T arg) where T : Window
        {
            arg.Show();
            App.Current.MainWindow = arg;
        }

        private void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            if (this.ListBox.SelectedItem == null)
            {
                this.DeleteClient.IsEnabled = false;
                return;
            }
            if (MessageBox.Show("Do you want to delete this record?", "Please confirm",  MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                return;
            msContext _context = new msContext();
            int id = int.Parse(this.ListBox.SelectedItem.ToString().Substring(3).Split(',')[0]);
            msClient _client = _context.msClients.First(cl => cl.ClientId ==id );
            _context.Entry(_client).State = System.Data.Entity.EntityState.Deleted;
            _context.SaveChanges();
            UpdateActions.textChanged(this);
        }
        private void ClientReport_Click(object sender, EventArgs e) => ReportClick("Generating Client Report...", Enums.ReportType.ClientReport);
        private  void PaymentReport_Click(object sender, EventArgs e) => ReportClick("Generating Payment Report...", Enums.ReportType.PaymentReport);
        private void UserReport_Click(object sender, EventArgs e)=> ReportClick("Generating User Report...", Enums.ReportType.UserReport);
        public async void ReportClick(string title, Enums.ReportType reportType)
        {
            UpdateActions.disable(this, title);
            await ReportManager.GenerateAsync(reportType);
            UpdateActions.enable(this, _defWindowTitle);
            MessageBox.Show(this, "Done please check documents","Done", MessageBoxButton.OK);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            msContext _context = new msContext();
            msUser user = _context.msUsers.First(us => us.UserId == msUser.CurrentUser.UserId);
            DateTime start = DateTime.Parse(user.UserLastVisitStart);
            DateTime end = DateTime.Now;
            user.TotalHours += (start - end).TotalHours;
            user.UserLastVisitEnd = DateTime.Now.ToString();
            _context.SaveChanges();
        }
    }
}
