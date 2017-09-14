using System.Collections.Generic;

namespace InmemDb.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public List<CartItem> CartItem { get; set; }
    }
}