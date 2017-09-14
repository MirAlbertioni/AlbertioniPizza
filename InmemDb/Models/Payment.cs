using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ShippingAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string NameOfcard { get; set; }
        public string CardNumber { get; set; }
        public string MMYY { get; set; }
        public string CVC { get; set; }

        public List<CartItem> CartItem { get; set; }
        public List<Cart> CartList { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }
    }
}
