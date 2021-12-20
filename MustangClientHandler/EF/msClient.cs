using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustangClientHandler.EF
{
    [Table("msClient")]
    public class msClient
    {
        [Key]
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public override string ToString() => $"Id: {this.ClientId}, Name : {this.ClientName}";
    }
}
