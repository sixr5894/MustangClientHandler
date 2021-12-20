using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustangClientHandler.EF
{
    [Table("msPayment")]
    public class msPayment
    {
        [Key]
        public string PaymentId { get; set; }
        public int ClientId { get; set; }
        public int PaymentType { get; set; }
        public string PaymentDate { get; set; }
        public int UserId { get; set; }
        public override string ToString() => $"Id : {this.PaymentId} , Client Id : {this.ClientId}, Date : {this.PaymentDate}, UserId {this.UserId}";
    }
}
