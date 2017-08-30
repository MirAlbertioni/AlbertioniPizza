using System.Collections.Generic;

namespace InmemDb.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public decimal OverallAmount { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<DishOrder> DishOrder { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}