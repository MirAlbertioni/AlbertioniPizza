using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class NewDishVM
    {
        public List<Category> Categories { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public Dish Dish { get; set; }
        public List<int> IngredientIds { get; set; }
    }
}
