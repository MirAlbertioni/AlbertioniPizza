using System.Collections.Generic;

namespace InmemDb.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public Dish Dish { get; set; }

        public virtual ICollection<DishCart> DishCart { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}