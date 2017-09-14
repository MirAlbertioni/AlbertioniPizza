using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class OrderConfirmation
    {
        [Key]
        public int OrderId { get; set; }
        public Payment Payment { get; set; }
    }
}
