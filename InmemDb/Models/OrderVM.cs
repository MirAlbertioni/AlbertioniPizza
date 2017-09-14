using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class OrderVM
    {
        public Payment GuestPayment { get; set; }
        public OrderConfirmation Order { get; set; }
    }
}
