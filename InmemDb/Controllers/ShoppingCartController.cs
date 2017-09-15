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
        private readonly CartService _cartService;

        public ShoppingCartController(ApplicationDbContext context, CartService cartService)
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

            return View("Cart", cartItems);
        }

        public IActionResult AddDishToCart(int dishId)
        {
            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                var newCart = _cartService.AddDishToNewCart(dishId, HttpContext);

                return RedirectToAction("Index", "Dish", newCart);
            }
            else
            {
                var currentCart = _cartService.AddDishToExistingCart(dishId, HttpContext);

                return RedirectToAction("Index", "Dish", currentCart);
            }
        }

        public IActionResult EditDishIngredientsInCart(int cartItemId, int dishId, int ingredientId)
        {
            var cartId = HttpContext.Session.GetInt32("Cart");
            var newDishVM = new DishIngredientVM();

            var ingredients = _context.Ingredients.ToList();

            var cartItem = _context.CartItems
                .Include(x => x.Dish)
                .Include(i => i.CartItemIngredient)
                .SingleOrDefault(x => x.CartId == cartId && x.Dish.DishId == dishId && x.CartItemId == cartItemId);

            var selectedIngredients = new List<SelectedIngredients>();

            foreach (var item in ingredients)
            {
                var selected = new SelectedIngredients
                {
                    Name = item.Name,
                    Id = item.IngredientId,
                    Price = item.Price
                };

                if (cartItem.CartItemIngredient.Any(x => x.Ingredient.IngredientId == item.IngredientId))
                {
                    selected.Enabled = true;
                }
                if (_context.DishIngredients.Any(di => di.DishId == dishId && di.IngredientId == ingredientId))
                    selected.Price = 0;
                selectedIngredients.Add(selected);
            }


            newDishVM.Dish = cartItem.Dish;
            newDishVM.SelectedIngredients = selectedIngredients;
            newDishVM.DishId = cartItem.Dish.DishId;
            newDishVM.CartItemId = cartItem.CartItemId;

            return View("EditDishIngredientsInCart", newDishVM);
        }

        [HttpPost]
        public IActionResult EditDishIngredientsInCart(DishIngredientVM dishIngredientVM)
        {

            var cartId = HttpContext.Session.GetInt32("Cart");

            var cartItem = _context.CartItems
                .Include(x => x.Dish)
                .Include(i => i.CartItemIngredient)
                .ThenInclude(s => s.Ingredient)
                .SingleOrDefault(x => x.CartId == cartId && x.Dish.DishId == dishIngredientVM.DishId && x.CartItemId == dishIngredientVM.CartItemId);

            var existingIngredients = cartItem.CartItemIngredient.Select(x => x.IngredientId).ToList();

            var SelectedIds = dishIngredientVM.SelectedIngredients.Where(x => x.Enabled == true).Select(s => s.Id).ToList();

            var unCheckedIds = dishIngredientVM.SelectedIngredients.Where(x => x.Enabled == false).Select(s => s.Id).ToList();


            var ingredientsToAdd = _context.Ingredients.Where(x => SelectedIds.Contains(x.IngredientId)).ToList();

            var ingredientsToRemove = _context.Ingredients.Where(x => unCheckedIds.Contains(x.IngredientId)).ToList();


            cartItem.CartItemIngredient.ForEach(i => _context.Remove(i));

            _context.SaveChangesAsync();
            foreach (var item in ingredientsToAdd)
            {
                var newIngredient = new CartItemIngredient
                {
                    CartItem = cartItem,
                    Ingredient = item,
                    CartItemId = cartItem.CartItemId,
                    IngredientId = item.IngredientId,
                    CartItemIngredientPrice = item.Price

                };
                cartItem.CartItemIngredient.Add(newIngredient);
            }

            _context.SaveChangesAsync();
            return RedirectToAction("Cart", "ShoppingCart");
        }

        public IActionResult ResetCart()
        {
            var session = HttpContext.Session;
            session.Remove("Cart");
            return RedirectToAction("Index", "Dish");
        }
    }
}
