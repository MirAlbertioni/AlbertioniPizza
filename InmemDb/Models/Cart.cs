using System.Collections.Generic;

namespace InmemDb.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public decimal OverallAmount { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<DishCart> DishOrder { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}