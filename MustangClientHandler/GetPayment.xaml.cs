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
    /// Interaction logic for GetPayment.xaml
    /// </summary>
    public partial class GetPayment : Window
    {
        bool onIntialize = false;
        int ClientID { get; }
        const string defText = "Enter sum";
        public GetPayment()
        {
            onIntialize = true;
            InitializeComponent();
            onIntialize = false;
            this.ConfirmButton.IsEnabled = false;
            this.Topmost = true;
        }
        public GetPayment(int arg):this()
        {
            this.ClientID = arg;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            int sum = int.Parse(this.SumText.Text);
            msContext _context = new msContext();
            _context.msPayments.Add(new msPayment {PaymentId = Guid.NewGuid().ToString(), ClientId  = this.ClientID, UserId = msUser.CurrentUser.UserId, PaymentDate = DateTime.Now.ToString(), PaymentType = 0, PaymentSum = sum});
            _context.SaveChanges();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SumText_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.SumText.Text == defText)
            {
                this.SumText.Text = "";
                this.SumText.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void SumText_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.SumText.Text == "")
            {
                this.SumText.Text = defText;
                this.SumText.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void SumText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (onIntialize)
                return;
            string sumtxt = this.SumText.Text;
            if (string.IsNullOrEmpty(sumtxt) || sumtxt == defText)
            {
                this.ConfirmButton.IsEnabled = false;
                return;
            }
            int sum;
            if (int.TryParse(this.SumText.Text,out sum))
            {
                this.ConfirmButton.IsEnabled = true;
            }
            else
                this.ConfirmButton.IsEnabled = false;
        }
    }
}
