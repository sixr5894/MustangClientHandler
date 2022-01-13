using Microsoft.VisualStudio.TestTools.UnitTesting;
using MustangClientHandler.EF;
using System;
using System.Linq;
using System.Threading;
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.Factory;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.Utility;
using TestStack.White.WindowsAPI;

namespace TestingMustangHandler
{
    [TestClass]
    public class UILoginTest 
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static Button loginButton;
        public static Window window;
        public static Application app;
        public static TextBox textBoxLogin;
        public static TextBox passwordBox;
        public static Label textBlockHeading;
        [TestInitialize]
        public void Initializator()
        {
            app = Application.Launch("MustangClientHandler.exe");
            ParticularInitialization();
            log.Info("Login test method initialized");
        }
        [TestMethod]
        public void TestLogin()
        {
            for(int i = 0; i < 20; i++)
            {
                string pass = RandomString(i + 1);
                textBoxLogin.Text = RandomString(i + 1);
                passwordBox.Text = pass;
                loginButton.Click();
                if (textBlockHeading.Text != "Wrong login or password" & i > 0)
                {
                    Assert.Fail();
                    log.Info($"Fail --- login = {textBoxLogin.Text} , password = {pass}");
                }
                else
                    log.Info($"Success --- login = {textBoxLogin.Text} , password = {pass}");
            }
            LogIn();
        }
        [TestCleanup]
        public void Complete()
        {
            Thread.Sleep(500);
            app.Kill();
        }
        public void LogIn()
        {
            textBoxLogin.Text = "admin";
            passwordBox.Text = "test";
            loginButton.Click();
            app.WaitWhileBusy();
        }
        void ParticularInitialization()
        {
            window = app.GetWindow("Login", InitializeOption.NoCache);
            loginButton = window.Get<Button>("button1");
            textBoxLogin = window.Get<TextBox>("textBoxLogin");
            passwordBox = window.Get<TextBox>("passwordBox");
            textBlockHeading = window.Get<Label>("textBlockHeading");
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
