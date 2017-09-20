using InmemDb.Data;
using InmemDb.Models;
using InmemDb.Models.ManageViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InmemDb.Services
{
    public class PaymentService : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<PaymentViewModel> PaymentGet(HttpContext httpContext)
        {
            var cartId = (int)httpContext.Session.GetInt32("Cart");
            var user = await _userManager.GetUserAsync(User);
            
            var newUser = new PaymentViewModel
            {
                User = new ApplicationUser
                {
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Address = user.Address,
                    Zip = user.Zip,
                    City = user.City,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                }
            };
            return newUser;
        }
    }
}
