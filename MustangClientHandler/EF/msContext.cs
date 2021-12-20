using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustangClientHandler.EF
{
    public class msContext: DbContext
    {
        public msContext() : this("DefaultConnection") { }
        public msContext(string arg) : base(arg)
        {
            //Database.SetInitializer<msContext>(null);
        }
        
        public DbSet<msClient> msClients { get; set; }
        public DbSet<msUser> msUsers { get; set; }
        public DbSet<msPayment> msPayments { get; set; }
    }
}
