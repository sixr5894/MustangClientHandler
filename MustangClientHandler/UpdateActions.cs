using MustangClientHandler.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MustangClientHandler
{
    public static class UpdateActions
    {
        public static Func<msClient, string, bool> containsPredicate = (client, arg) => client.ClientName.Contains(arg) || client.ClientId.ToString().Contains(arg);
        public static Action<MainWindow> onFocus = (arg) =>
        {
            if (arg.SearchText.Text == MainWindow._defaultSearchText)
            {
                arg.SearchText.Text = "";
                arg.SearchText.Foreground = new SolidColorBrush(Colors.Black);
            }
        };
        public static Action<MainWindow> focusLost = (arg) =>
        {
            if (arg.SearchText.Text == "")
            {
                arg.SearchText.Text = MainWindow._defaultSearchText;
                arg.SearchText.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        };
        public static Action<MainWindow> textChanged = (arg) =>
        {
            string SrText = arg.SearchText.Text;
            arg.ListBox.Items.Clear();
            var list = new msContext().msClients.ToList();
            if (SrText != MainWindow._defaultSearchText)
                list = list.Where(cl => containsPredicate(cl, SrText)).ToList();

            foreach (var client in list)
            {
                arg.ListBox.Items.Add(client.ToString());
            }
        };
        public static Action<MainWindow, bool> setButtons = (win, arg) =>
        {
            win.DeleteClient.IsEnabled = win.UserInAdminRole ? arg : false;
            win.GetPayment.IsEnabled = arg;
        };
    }
}
