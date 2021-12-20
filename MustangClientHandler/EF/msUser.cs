using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustangClientHandler.EF
{
    [Table("msUser")]
    public class msUser
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int UserRole { get; set; }
        public string UserLastVisitStart { get; set; }
        public string UserLastVisitEnd { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public double TotalHours { get; set; }
        public override string ToString() => $"name : {this.UserName}, login : {this.UserLogin}, role : {this.UserRole}";
        public static msUser CurrentUser { get; set; }
    }
}
