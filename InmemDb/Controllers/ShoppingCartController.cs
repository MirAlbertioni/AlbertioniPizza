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
            //if (session.GetString("Login") == null)
            //{
            //    return PartialView("_LoginPartial");
            //}
            //else
            //{
            var session = HttpContext.Session;

            var selectedProd = _context.Dishes.SingleOrDefault(x => x.DishId == id);

            Order order;

            if (session.GetString("Dish") == null)
            {
                order = new Order { DishOrder = new List<DishOrder>() };
            }
            else
            {
                var temp = session.GetString("Dish");
                order = JsonConvert.DeserializeObject<Order>(temp);
            }

            DishOrder dishOrder = new DishOrder
            {
                Quantity = 1,
                Dish = selectedProd,
                DishId = selectedProd.DishId,
            };

            if (order.DishOrder.Any(x => x.DishId == selectedProd.DishId))
            {
                order.DishOrder.First(c => c.DishId == selectedProd.DishId).Quantity++;
            }
            else
            {
                order.DishOrder.Add(dishOrder);
            }

            //Lägg tillbaka listan i en sessionsvariabel
            var serializedValue = JsonConvert.SerializeObject(order);
            session.SetString("Dish", serializedValue);

            //Skicka till en partial view som visar summaryn
            return PartialView("_OrderSummary", order.DishOrder);
            //}
        }

        public IActionResult OrderDetails(ISession session)
        {
            var applicationUser = new ApplicationUser();

            var temp = session.GetString("Dish");
            var order = JsonConvert.DeserializeObject<Order>(temp);

            var temp2 = session.GetString("Login");
            var currentUser = JsonConvert.DeserializeObject<ApplicationUser>(temp2);

            applicationUser = currentUser;

            Order newOrder = new Order();
            DishOrder newDishOrder = new DishOrder();
            //Bestallning nyBestallning = new Bestallning();
            //BestallningMatratt nyBestallningMatratt = new BestallningMatratt();

            newOrder.OverallAmount = order.DishOrder.Sum(x => x.Quantity * x.Dish.Price);
            newOrder.UserId = applicationUser.Id;

            _context.Order.Add(newOrder);
            _context.SaveChanges();

            foreach (var dishOrder in order.DishOrder)
            {
                newDishOrder.Quantity = dishOrder.Quantity;
                newDishOrder.Order = newOrder;
                newDishOrder.DishId = dishOrder.DishId;

                _context.DishOrder.Add(newDishOrder);
                _context.SaveChanges();
            }
            OrderViewModel orderViewModel = new OrderViewModel
            {
                User = applicationUser,
                Order = newOrder,
                DishOrder = newDishOrder
            };

            return View(orderViewModel);
        }

        public IActionResult ResetCart()
        {
            var session = HttpContext.Session;
            session.Remove("Dish");
            return RedirectToAction("Index", "Home");
        }
    }
}
