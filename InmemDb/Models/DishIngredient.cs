using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class DishIngredient
    {
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public List<Ingredient> IngredientList { get; set; }
        public List<Dish> DishList { get; set; }
        public bool Enabled { get; set; }
    }
}
