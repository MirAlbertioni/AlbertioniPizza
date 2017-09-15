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

        public async Task<Payment> PaymentCustomerPost(PaymentViewModel model, HttpContext httpContext)
        {
            var cartId = (int)httpContext.Session.GetInt32("Cart");
            Cart cart;
            List<CartItem> cartItems;

            cart = _context.Carts
                    .Include(i => i.CartItem)
                    .ThenInclude(x => x.CartItemIngredient)
                    .ThenInclude(ig => ig.Ingredient)
                    .Include(i => i.CartItem)
                    .ThenInclude(ci => ci.Dish)
                    .SingleOrDefault(x => x.CartId == cartId);

            cartItems = cart.CartItem;

            var newPayment = new Payment()
            {
                PaymentId = model.Payment.PaymentId,
                FirstName = model.Payment.FirstName,
                LastName = model.Payment.LastName,
                ShippingAddress = model.Payment.ShippingAddress,
                ZipCode = model.Payment.ZipCode,
                Email = model.Payment.Email,
                PhoneNumber = model.Payment.PhoneNumber,
                City = model.Payment.City,
                CartId = cartId,
                Cart = cart,
                CartItem = cartItems,
                NameOfcard = model.Payment.NameOfcard,
                CardNumber = model.Payment.CardNumber,
                MMYY = model.Payment.MMYY,
                CVC = model.Payment.CVC
            };

            await _context.Payment.AddAsync(newPayment);
            await _context.SaveChangesAsync();

            return newPayment;
        }
    }
}
