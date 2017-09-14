using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class DishIngredientVM
    {
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public List<SelectedIngredients> SelectedIngredients { get; set; }
        public int CartItemId { get; set; }
    }
}
