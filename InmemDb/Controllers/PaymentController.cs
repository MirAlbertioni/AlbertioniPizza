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
        private readonly SignInManager<ApplicationUser> _signInManager;

        public PaymentController(UserManager<ApplicationUser> userManager, ApplicationDbContext context
            , PaymentService paymentService, ShoppingCartService cartService, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _paymentService = paymentService;
            _cartService = cartService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Payment()
        {
            if (User.Identity.IsAuthenticated)
            {
                var cartId = (int)HttpContext.Session.GetInt32("Cart");
                var user = await _userManager.GetUserAsync(User);

                var newUser = new PaymentViewModel
                {
                    Register = new Register
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
                return View(newUser.Register);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Payment(Register model)
        {
            if(ModelState.IsValid)
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

                if (User.Identity.IsAuthenticated)
                {
                    var user = await _userManager.GetUserAsync(User);

                    var newModel = new PaymentViewModel
                    {
                        Register = model
                    };
                    newModel.Register.CartItem = cart.CartItem;
                    newModel.Register.Cart = cart;
                    newModel.Register.CartId = cartId;
                    await _context.Register.AddAsync(newModel.Register);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("OrderConfirmation", "OrderConfirmation", newModel);
                }
                else
                {
                    model.CartId = cartId;
                    model.Cart = cart;
                    model.CartItem = cartItems;
                    await _context.Register.AddAsync(model);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("OrderConfirmation", "OrderConfirmation", model);
                }
            }
            return View();
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

        public IActionResult RemoveProduct(int dishId, int cartItemId)
        {
            var cartId = HttpContext.Session.GetInt32("Cart");

            var cart = _context.Carts.Include(x => x.CartItem).SingleOrDefaultAsync(a => a.CartId == cartId);

            var cartItem = _context.CartItems
                .Include(x => x.Dish)
                .Include(i => i.CartItemIngredient)
                .ThenInclude(s => s.Ingredient).SingleOrDefault(x => x.CartId == cartId && x.Dish.DishId == dishId && x.CartItemId == cartItemId);

            _context.CartItems.Remove(cartItem);
            _context.SaveChangesAsync();

            return RedirectToAction("Checkout", "Payment");
        }

        public IActionResult EditDishIngredientsInCheckout(int cartItemId, int dishId)
        {
            var editInCartGet = _cartService.EditDishIngredientsInCartGet(cartItemId, dishId, HttpContext);
            return PartialView("_EditDishIngredientsInCheckout", editInCartGet);
        }

        [HttpPost]
        public IActionResult EditDishIngredientsInCheckout(DishIngredientVM dishIngredientVM)
        {
            _cartService.EditDishIngredientsInCartPost(dishIngredientVM, HttpContext);
            return RedirectToAction("Checkout", "Payment");
        }
    }
}