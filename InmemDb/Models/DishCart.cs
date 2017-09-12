using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class DishCart
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public ApplicationUser User { get; set; }
        public Dish Dish { get; set; }
        public bool Enabled { get; set; }

        public List<DishIngredient> DishIngredients { get; set; }
    }
}