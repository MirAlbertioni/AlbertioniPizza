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
        private readonly IngredientService _ingredientService;

        public ShoppingCartController(ApplicationDbContext context, IngredientService ingredientService)
        {
            _context = context;
            _ingredientService = ingredientService;
        }

        // GET: Dishes
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Cart()
        {
            var dishes = _context.DishCart
                .Include(x => x.Dish)
                .ThenInclude(i => i.DishIngredients).ToList();
            return View(dishes);
        }

        public IActionResult AddDishToCart(int id)
        {
            var session = HttpContext.Session;

            var selectedProd = _context.Dishes.SingleOrDefault(x => x.DishId == id);

            Cart cart;

            if (session.GetString("Cart") == null)
            {
                cart = new Cart { DishCart = new List<DishCart>() };
                _context.Cart.Add(cart);
                _context.SaveChanges();
            }
            else
            {
                var temp = session.GetString("Cart");
                cart = JsonConvert.DeserializeObject<Cart>(temp);
            }

            DishCart dishCart = new DishCart
            {
                Quantity = 1,
                Dish = selectedProd,
                DishId = selectedProd.DishId,
                DishIngredients = new List<DishIngredient>(),
            };

            cart.DishCart.Add(dishCart);
            _context.DishCart.Add(dishCart);
            _context.SaveChanges();
            var serializedValue = JsonConvert.SerializeObject(cart.CartId);
            session.SetString("Cart", serializedValue);

            return RedirectToAction("Index", "Dishes", cart.DishCart);
        }

        public IActionResult ResetCart()
        {
            var session = HttpContext.Session;
            session.Remove("Cart");
            return RedirectToAction("Index", "Dishes");
        }

        public IActionResult LoginCreateOrGuest()
        {
            return View();
        }
    }
}
