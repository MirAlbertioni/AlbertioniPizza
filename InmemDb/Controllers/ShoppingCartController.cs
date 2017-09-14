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

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cart()
        {
            Cart cart;
            List<CartItem> cartItems;

            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                cartItems = new List<CartItem>();
            }
            else
            {
                var cartId = (int)HttpContext.Session.GetInt32("Cart");

                cart = _context.Carts
                    .Include(i => i.CartItem)
                    .ThenInclude(x => x.CartItemIngredient)
                    .ThenInclude(ig => ig.Ingredient)
                    .Include(i => i.CartItem)
                    .ThenInclude(ci => ci.Dish)
                    .SingleOrDefault(x => x.CartId == cartId);

                cartItems = cart.CartItem;
            }

            return View("Cart", cartItems);
        }

        public IActionResult AddDishToCart(int dishId)
        {
            int cartId;
            var dish = _context.Dishes.Include(x => x.DishIngredients)
                    .ThenInclude(i => i.Ingredient)
                    .SingleOrDefault(d => d.DishId == dishId);

            CartItem cartItem = new CartItem();
            List<CartItemIngredient> cartItemIngredient = new List<CartItemIngredient>();

            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                foreach (var item in dish.DishIngredients)
                {
                    var newCartItemIngredient = new CartItemIngredient
                    {
                        CartItem = cartItem,
                        Ingredient = item.Ingredient,
                        IngredientId = item.IngredientId,
                        CartItemIngredientPrice = item.Ingredient.Price
                    };

                    cartItemIngredient.Add(newCartItemIngredient);
                }

                List<CartItem> listOfCartItems = new List<CartItem>
                {
                    cartItem
                };

                Cart newCart = new Cart();
                cartItem.Dish = dish;
                cartItem.CartId = newCart.CartId;
                cartItem.CartItemIngredient = cartItemIngredient;
                cartItem.Quantity = 1;

                newCart.CartItem = listOfCartItems;

                _context.Carts.Add(newCart);
                _context.SaveChanges();

                cartId = newCart.CartId;
                HttpContext.Session.SetInt32("Cart", cartId);

                return RedirectToAction("Index", "Dish", newCart.CartItem);
            }
            else
            {
                cartId = (int)HttpContext.Session.GetInt32("Cart");

                var currentCart = _context.Carts.Include(i => i.CartItem)
                    .ThenInclude(d => d.Dish)
                    .SingleOrDefault(x => x.CartId == cartId);


                foreach (var item in dish.DishIngredients)
                {
                    var newCartItemIngredient = new CartItemIngredient
                    {
                        CartItem = cartItem,
                        Ingredient = item.Ingredient,
                        IngredientId = item.IngredientId,
                        CartItemIngredientPrice = item.Ingredient.Price
                    };
                    cartItemIngredient.Add(newCartItemIngredient);
                }
                var newDish = new CartItem
                {
                    CartId = currentCart.CartId,
                    Dish = dish,
                    Quantity = 1,
                    CartItemIngredient = cartItemIngredient
                };

                _context.CartItems.Add(newDish);
                _context.SaveChangesAsync();

                return RedirectToAction("Index", "Dish", currentCart.CartItem);
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
