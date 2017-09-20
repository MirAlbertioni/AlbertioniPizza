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
        public async Task<ActionResult> OrderConfirmation(Register model)
        {
            var cartId = (int)HttpContext.Session.GetInt32("Cart");
            List<CartItem> cartItems;

            var currentCart = _context.Register.Include(x => x.Cart)
                .ThenInclude(i => i.CartItem)
                .ThenInclude(x => x.CartItemIngredient)
                .ThenInclude(ig => ig.Ingredient)
                .Include(i => i.CartItem)
                .ThenInclude(ci => ci.Dish)
                .SingleOrDefault(x => x.CartId == cartId);

            cartItems = currentCart.CartItem;
            if(User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                var newUser = new PaymentViewModel
                {
                    Register = currentCart,
                    Cart = currentCart.Cart,
                    User = user
                };
                return View(newUser);
            }
            var newGuest = new PaymentViewModel
            {
                Register = model,
                Cart = currentCart.Cart
            };

            return View(newGuest);
        }
    }
}