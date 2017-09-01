using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string NameOfcard { get; set; }
        public int CardNumber { get; set; }
        public int MMYY { get; set; }
        public int CVC { get; set; }
        public ApplicationUser User { get; set; }
    }
}
