using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public ApplicationUser Applicationuser { get; set; }
        public int ApplicationUserId { get; set; }
        public List<CartItem> Items { get; set; }
    }
}
