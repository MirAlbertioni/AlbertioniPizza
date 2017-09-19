using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using InmemDb.Data;
using InmemDb.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Session;
using InmemDb.Services;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InmemDb.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCartService _cartService;

        public ShoppingCartController(ApplicationDbContext context, ShoppingCartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        // GET: Dishes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cart()
        {
            List<CartItem> cartItems;

            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                cartItems = new List<CartItem>();
            }
            else
            {
                var cartItemsInService = _cartService.Cart(HttpContext);
                cartItems = cartItemsInService;
            }

            return PartialView("_Cart", cartItems);
        }

        public IActionResult AddDishToCart(int dishId)
        {
            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                var newCart = _cartService.AddDishToNewCart(dishId, HttpContext);

                return PartialView("_Cart", newCart);
            }
            else
            {
                var currentCart = _cartService.AddDishToExistingCart(dishId, HttpContext);

                return PartialView("_Cart", currentCart);
            }
        }

        public IActionResult EditDishIngredientsInCart(int cartItemId, int dishId)
        {
            var editInCartGet = _cartService.EditDishIngredientsInCartGet(cartItemId, dishId, HttpContext);
            return PartialView("_EditDishIngredientsInCart", editInCartGet);
        }

        [HttpPost]
        public IActionResult EditDishIngredientsInCart(DishIngredientVM dishIngredientVM)
        {
            _cartService.EditDishIngredientsInCartPost(dishIngredientVM, HttpContext);
            return RedirectToAction("Index", "Dish");
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

            return PartialView("_Cart");
        }

        public IActionResult ResetCart()
        {
            var session = HttpContext.Session;
            session.Remove("Cart");
            return RedirectToAction("Index", "Dish");
        }
    }
}
