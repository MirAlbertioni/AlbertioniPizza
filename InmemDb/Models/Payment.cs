using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Firstname")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Lastname")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Address")]
        public string ShippingAddress { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 5)]
        [Display(Name = "Zipcode")]
        public string ZipCode { get; set; }

        [Required]
        [Phone]
        [StringLength(10, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 10)]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Full name")]
        public string NameOfcard { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 16)]
        [Display(Name = "Card number")]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(4, ErrorMessage = "The {0} must be {2} characters long.", MinimumLength = 4)]
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
