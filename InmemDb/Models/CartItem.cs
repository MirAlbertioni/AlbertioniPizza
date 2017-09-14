using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public Dish Dish { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public List<CartItemIngredient> CartItemIngredient { get; set; }
    }
}