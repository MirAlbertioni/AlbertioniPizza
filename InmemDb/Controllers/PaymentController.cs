using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InmemDb.Models;
using InmemDb.Data;
using Microsoft.EntityFrameworkCore;
using InmemDb.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using InmemDb.Services;

namespace InmemDb.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaymentService _paymentService;
        private readonly ShoppingCartService _cartService;

        public PaymentController(UserManager<ApplicationUser> userManager, ApplicationDbContext context
            , PaymentService paymentService, ShoppingCartService cartService)
        {
            _userManager = userManager;
            _context = context;
            _paymentService = paymentService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Payment()
        {
            var cartId = (int)HttpContext.Session.GetInt32("Cart");
            var user = await _userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
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
                return View(newUser);
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Payment(PaymentViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var cartId = (int)HttpContext.Session.GetInt32("Cart");
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

                var user = await _userManager.GetUserAsync(User);

                var newModel = new PaymentViewModel
                {
                    Payment = new Payment
                    {
                        FirstName = user.Firstname,
                        LastName = user.Lastname,
                        ShippingAddress = user.Address,
                        ZipCode = user.Zip,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        CartId = cartId,
                        Cart = cart,
                        CartItem = cartItems,
                        NameOfcard = model.Payment.NameOfcard,
                        CardNumber = model.Payment.CardNumber,
                        MMYY = model.Payment.MMYY,
                        CVC = model.Payment.CVC
                    }
                };
                await _context.Payment.AddAsync(newModel.Payment);
                await _context.SaveChangesAsync();

                return RedirectToAction("OrderConfirmation", "OrderConfirmation", newModel);
            }
            else
            {
                var newPayment = await _paymentService.PaymentCustomerPost(model, HttpContext);

                return RedirectToAction("OrderConfirmation", "OrderConfirmation", newPayment);
            }
        }

        public async Task<IActionResult> Checkout(int paymentId)
        {
            var cartId = (int)HttpContext.Session.GetInt32("Cart");

            var currentCart = await _context.Carts
                .Include(i => i.CartItem)
                .ThenInclude(x => x.CartItemIngredient)
                .ThenInclude(ig => ig.Ingredient)
                .Include(i => i.CartItem)
                .ThenInclude(ci => ci.Dish)
                .SingleOrDefaultAsync(x => x.CartId == cartId);

            return View(currentCart);
        }
    }
}