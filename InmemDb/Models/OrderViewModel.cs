using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class OrderViewModel
    {
        public ApplicationUser User { get; set; }
        public DishCart DishCart { get; set; }
        public Cart Order { get; set; }
    }
}
