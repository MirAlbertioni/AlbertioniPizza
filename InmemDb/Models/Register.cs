using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class Register
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 5)]
        [Display(Name = "Zipcode")]
        public string Zip { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Phone]
        [StringLength(10, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 10)]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Full name")]
        public string NameOfcard { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 16)]
        [Display(Name = "Card number")]
        public string CardNumber { get; set; }

        [Required]
        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}")]
        [StringLength(5, ErrorMessage = "Type MM/YY", MinimumLength = 5)]
        [Display(Name = "MMYY")]
        public string MMYY { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 3)]
        [Display(Name = "CVC")]
        public string CVC { get; set; }

        public List<CartItem> CartItem { get; set; }
        public List<Cart> CartList { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }
    }
}
