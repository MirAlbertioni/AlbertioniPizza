using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class NewDishViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public Dish Dish { get; set; }
        public List<int> IngredientsId { get; set; }
    }
}
