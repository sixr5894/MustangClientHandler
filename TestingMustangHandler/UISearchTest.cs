using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.Factory;
using TestStack.White.UIItems.ListBoxItems;
using MustangClientHandler.EF;
using System.Threading;

namespace TestingMustangHandler
{
    [TestClass]
    public class UISearchTest
    {
        private static Window window;
        private static Application application;
        private static TextBox SearchText;
        private static ListBox ListBox;
        private static List<msClient> clientsList = new List<msClient>();
        [TestInitialize]
        public void Initialize()
        {
            UILoginTest temp = new UILoginTest();
            temp.Initializator();
            temp.LogIn();
            application = Application.Attach("MustangClientHandler");
            window = application.GetWindow("MainWindow");
            SearchText = window.Get<TextBox>("SearchText");
            ListBox = window.Get<ListBox>("ListBox");
        }
        [TestMethod]
        public void BruteForceSearch()
        {
            PrepareRandomUsers();
            foreach (msClient client in clientsList)
            {
                SearchText.Text = client.ClientName;
                application.WaitWhileBusy();
                Thread.Sleep(100);
                if (TextNotReflected(client.ClientName))
                    Assert.Fail();
            }
        }
        [TestCleanup]
        public void Clear()
        {
            msContext context = new msContext();
            foreach (msClient client in clientsList)
                context.msClients.Remove(context.msClients.Where(cl => cl.ClientName == client.ClientName).First());
            context.SaveChanges();
            application.Kill();
        }
        bool TextNotReflected(string text) => ListBox.Items.Where(item => item.Text.Contains(text)).FirstOrDefault() == null;
        void PrepareRandomUsers()
        {
            msContext context = new msContext();
            for (int i = 1; i < 50; i++)
            {
                msClient client = new msClient() { ClientName = UILoginTest.RandomString(i) };
                clientsList.Add(client);
                context.msClients.Add(client);
            }
            context.SaveChanges();
        }

    }
}
