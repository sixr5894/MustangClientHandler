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
    /// Interaction logic for AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        public bool onInitialize = false;
        public const string defText = "Type name";
        public AddClient()
        {
            onInitialize = true;
            InitializeComponent();
            onInitialize = false;
            this.ConfirmButton.IsEnabled = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (onInitialize)
                return;
            if (this.NameText.Text == ""|| this.NameText.Text == defText)
                this.ConfirmButton.IsEnabled = false;
            else
                this.ConfirmButton.IsEnabled = true;
        }

        private void NameText_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.NameText.Text == defText)
            {
                this.NameText.Text = "";
                this.NameText.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void NameText_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            msContext _context = new msContext();
            _context.msClients.Add(new msClient { ClientName = this.NameText.Text });
            _context.SaveChanges();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
