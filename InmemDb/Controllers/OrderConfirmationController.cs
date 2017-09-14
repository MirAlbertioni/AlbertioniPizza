using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InmemDb.Data;
using Microsoft.EntityFrameworkCore;
using InmemDb.Models;
using InmemDb.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;

namespace InmemDb.Controllers
{
    public class OrderConfirmationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public OrderConfirmationController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        // GET: OrderConfirmationGuest
        public async Task<ActionResult> OrderConfirmation(int paymentId, OrderConfirmation order)
        {
            var cartId = (int)HttpContext.Session.GetInt32("Cart");
            var user = await _userManager.GetUserAsync(User);
            

            if (User.Identity.IsAuthenticated)
            {
                var currentPayment = await _context.Payment.SingleOrDefaultAsync(x => x.PaymentId == paymentId);

                var currentGuest = await _context.Payment
                .Include(x => x.Cart)
                .ThenInclude(x => x.CartItem)
                .ThenInclude(x => x.CartItemIngredient)
                .ThenInclude(x => x.Ingredient)
                .Include(x => x.CartItem)
                .ThenInclude(x => x.Dish)
                .SingleOrDefaultAsync(x => x.PaymentId == currentPayment.PaymentId && x.CartId == cartId);

                var newOrder = new OrderConfirmation
                {
                    Payment = currentGuest
                };

                //var session = HttpContext.Session;
                //session.Remove("Cart");
                return View(newOrder);
            }
            else
            {
                var currentGuest = await _context.Payment
                .Include(x => x.Cart)
                .ThenInclude(x => x.CartItem)
                .ThenInclude(x => x.CartItemIngredient)
                .ThenInclude(x => x.Ingredient)
                .Include(x => x.CartItem)
                .ThenInclude(x => x.Dish)
                .SingleOrDefaultAsync(x => x.PaymentId == paymentId && x.CartId == cartId);

                var currentCart = currentGuest.CartItem.ToList();

                var newOrder = new OrderConfirmation
                {
                    Payment = currentGuest
                };

                order = newOrder;

                //var session = HttpContext.Session;
                //session.Remove("Cart");

                return View(newOrder);
            }
        }
    }
}