using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class Ingridient
    {
        public int IngridientId { get; set; }
        public string Name { get; set; }
        public List<DishIngridient> DishIngridients { get; set; }

    }
}
