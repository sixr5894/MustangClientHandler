using Microsoft.VisualStudio.TestTools.UnitTesting;
using MustangClientHandler;
using MustangClientHandler.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UnitTesting.MustangClientHandler
{
    [TestClass]
    public class UpdateActionsTest
    {
        const string _searchText = "a";
        MainWindow mainwindow;
        [TestInitialize]
        public void Initialize()
        {
            mainwindow = new MainWindow();
        }
        [TestMethod]
        public void UpdateElements()
        {
            OnFocus();

            FocusLost();

            TextChanged();

            DisableWindow();

            EnableWindow();
        }
        void DisableWindow()
        {
            UpdateActions.disableWindow(mainwindow, "disabled");
            if (mainwindow.IsEnabled)
                Assert.Fail();
            if (mainwindow.Title != "disabled")
                Assert.Fail();
        }

        void EnableWindow()
        {
            UpdateActions.enableWindow(mainwindow);
            if (mainwindow.IsEnabled == false)
                Assert.Fail();
            if (mainwindow.Title != MainWindow._defWindowTitle)
                Assert.Fail();
        }
        void TextChanged()
        {
            mainwindow.SearchModule.Text = _searchText;
            UpdateActions.textChanged(mainwindow);
            List<msClient> clientList = new msContext().msClients.ToList().Where(cl => UpdateActions.containsPredicate(cl, _searchText)).ToList();
            List<string> updatedList = mainwindow.GetListBoxContent();
            foreach (var temp in clientList)
            {
                if (updatedList.FirstOrDefault(c => c == temp.ToString()) == null)
                    Assert.Fail();
            }
        }
        void FocusLost()
        {
            mainwindow.SearchModule.Text = "";
            UpdateActions.focusLost(mainwindow);
            if (mainwindow.SearchModule.Text != MainWindow._defaultSearchText)
                Assert.Fail();
            if (GetColor(mainwindow.SearchModule.Foreground) != Colors.LightGray)
                Assert.Fail();
        }
        void OnFocus()
        {
            UpdateActions.onFocus(mainwindow);
            if (mainwindow.SearchModule.Text != "")
                Assert.Fail();
            if (GetColor(mainwindow.SearchModule.Foreground) != Colors.Black)
                Assert.Fail();
        }
        Color GetColor(Brush brush) => (brush as SolidColorBrush).Color;

    }
}
