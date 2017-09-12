using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
        public Ingredient Ingredient { get; set; }
        public int IngredientId { get; set; }
        public DishCart DishCart { get; set; }
    }
}
