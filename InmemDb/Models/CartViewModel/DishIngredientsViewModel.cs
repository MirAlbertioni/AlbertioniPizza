using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models.CartViewModel
{
    public class DishIngredientsViewModel
    {
        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public List<CheckBox> Checkboxes { get; set; }
        public int CartItemId { get; set; }
    }
}
