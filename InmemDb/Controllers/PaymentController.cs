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

namespace InmemDb.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Payment()
        {
            var cartId = (int)HttpContext.Session.GetInt32("Cart");
            var user = await _userManager.GetUserAsync(User);
            if (User.IsInRole("User"))
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

            if (User.IsInRole("User"))
            {
                var user = await _userManager.GetUserAsync(User);

                var newModel = new PaymentViewModel
                {
                    User = new ApplicationUser
                    {
                        Firstname = model.Payment.FirstName,
                        Lastname = model.Payment.LastName,
                        Address = model.Payment.ShippingAddress,
                        Zip = model.Payment.ZipCode,
                        City = model.Payment.City,
                        Email = model.Payment.Email,
                        PhoneNumber = model.Payment.PhoneNumber

                    },
                    Payment = new Payment
                    {
                        CartId = cartId,
                        Cart = cart,
                        CartItem = cartItems,
                        NameOfcard = model.Payment.NameOfcard,
                        CardNumber = model.Payment.CardNumber,
                        MMYY = model.Payment.MMYY,
                        CVC = model.Payment.CVC
                    }
                };
                return RedirectToAction("OrderConfirmation", "OrderConfirmation", newModel);
            }
            else
            {
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

                return RedirectToAction("OrderConfirmation", "OrderConfirmation", newPayment);
            }
        }

        public IActionResult LoginCreateOrGuest()
        {
            return View();
        }
    }
}