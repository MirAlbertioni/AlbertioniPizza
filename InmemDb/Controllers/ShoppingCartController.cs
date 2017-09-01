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
        // GET: /<controller>/
        public IActionResult AddProduct(int id)
        {
            var session = HttpContext.Session;

            var selectedProd = _context.Dishes.SingleOrDefault(x => x.DishId == id);

            Cart order;

            if (session.GetString("Dish") == null)
            {
                order = new Cart { DishCart = new List<DishCart>() };
            }
            else
            {
                var temp = session.GetString("Dish");
                order = JsonConvert.DeserializeObject<Cart>(temp);
            }

            DishCart dishOrder = new DishCart
            {
                Quantity = 1,
                Dish = selectedProd,
                DishId = selectedProd.DishId,
            };

            if (order.DishCart.Any(x => x.DishId == selectedProd.DishId))
            {
                order.DishCart.First(c => c.DishId == selectedProd.DishId).Quantity++;
            }
            else
            {
                order.DishCart.Add(dishOrder);
            }

            var serializedValue = JsonConvert.SerializeObject(order);
            session.SetString("Dish", serializedValue);

            return PartialView("_OrderSummary", order.DishCart);
        }

        public IActionResult ResetCart()
        {
            var session = HttpContext.Session;
            session.Remove("Dish");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult LoginCreateOrGuest()
        {
            return View();
        }
    }
}
