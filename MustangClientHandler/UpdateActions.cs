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
        public static Func<msClient, string, bool> containsPredicate = (client, arg) => client.ClientName.ToLower().Contains(arg.ToLower()) || client.ClientId.ToString().Contains(arg);
        public static Action<MainWindow> onFocus = (window) =>
        {
            if (window.SearchText.Text == MainWindow._defaultSearchText)
            {
                window.SearchText.Text = "";
                window.SearchText.Foreground = new SolidColorBrush(Colors.Black);
            }
        };
        public static Action<MainWindow> focusLost = (window) =>
        {
            if (window.SearchText.Text == "")
            {
                window.SearchText.Text = MainWindow._defaultSearchText;
                window.SearchText.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        };
        public static Action<MainWindow> textChanged = (window) =>
        {
            string SrText = window.SearchText.Text;

            window.ListBox.Items.Clear();

            var list = new msContext().msClients.ToList();

            if (SrText != MainWindow._defaultSearchText)
                list = list.Where(cl => containsPredicate(cl, SrText)).ToList();

            foreach (var client in list)
                window.ListBox.Items.Add(client.ToString());
        };
        public static Action<MainWindow, bool> setButtons = (window, arg) =>
        {
            window.DeleteClient.IsEnabled = window.UserInAdminRole ? arg : false;
            window.GetPayment.IsEnabled = arg;
        };
        public static Action<MainWindow, string> disableWindow = (win, title) => setWindow(win, title, true);

        public static Action<MainWindow, string> enableWindow = (win, title) => setWindow(win, title, false);

        private static Action<MainWindow, string, bool> setWindow = (win, title, enable) =>
        {
            win.Title = title;
            win.IsEnabled = enable;
        };
    }
}
