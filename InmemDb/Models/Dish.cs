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
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
    }
}
