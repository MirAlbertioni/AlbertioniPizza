using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class DishCart
    {
        public int DishId { get; set; }
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public ApplicationUser User { get; set; }

        public virtual Dish Dish { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
