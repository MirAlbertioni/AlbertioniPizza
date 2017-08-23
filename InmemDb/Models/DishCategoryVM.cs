using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class DishCategoryVM
    {
        public Dish Dish { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
    }
}
