using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models.ManageViewModels
{
    public class PaymentViewModel
    {
        public Payment Payment { get; set; }
        public ApplicationUser User { get; set; }
        public Cart Cart { get; set; }
    }
}
