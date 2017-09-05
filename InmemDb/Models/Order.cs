using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ShippingAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int TotalPrice { get; set; }
    }
}
