using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models.ManageViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public Cart Cart { get; set; }
        public Register Register { get; set; }
    }
}
