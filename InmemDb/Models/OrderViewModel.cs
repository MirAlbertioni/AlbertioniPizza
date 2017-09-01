using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class OrderViewModel
    {
        public Payment Payment { get; set; }
        public DishCart DishCart { get; set; }
        public Cart Cart { get; set; }
        public ApplicationUser User { get; set; }
    }
}
