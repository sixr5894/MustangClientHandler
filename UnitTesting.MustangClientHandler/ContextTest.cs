using Microsoft.VisualStudio.TestTools.UnitTesting;
using MustangClientHandler.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTesting.MustangClientHandler
{
    [TestClass]
    public class ContextTest
    {
        msContext context;
        List<msClient> clientList;

        [TestInitialize]
        public void Initialize()
        {
            context = new msContext();
            clientList = new List<msClient>();
            for (int i = 0; i < 100; i++)
                clientList.Add(new msClient { ClientName = Inventory.RandomString(20) });
        }

        [TestMethod]
        public void TestСoherently()
        {
            InsertRandomUsers();

            ChangeInsertedUsers();

            DeleteInsertedClients();
        }

        [TestCleanup]
        public void Clean()
        {
            this.context = null;
            this.clientList = null;
        }
        void SaveChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch
            {
                Assert.Fail();
            }
        }

        void InsertRandomUsers()
        {
            foreach (msClient client in clientList)
                context.msClients.Add(client);
            SaveChanges();
        }

        void ChangeInsertedUsers()
        {
            context = new msContext();
            msClient client;
            foreach (msClient cl in clientList)
            {
                client = context.msClients.FirstOrDefault(c => c.ClientName == cl.ClientName);
                if (client == null)
                    Assert.Fail();
                client.ClientName += Inventory.RandomString(5);
                context.Entry<msClient>(client).State = System.Data.Entity.EntityState.Modified;
            }
            SaveChanges();
        }

        void DeleteInsertedClients()
        {
            msClient client;
            context = new msContext();
            foreach (msClient cl in clientList)
            {
                client = context.msClients.FirstOrDefault(c => c.ClientName.Contains(cl.ClientName));
                context.Entry<msClient>(client).State = System.Data.Entity.EntityState.Deleted;
            }
            SaveChanges();
        }

    }
}
