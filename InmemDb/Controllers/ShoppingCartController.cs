﻿using System;
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

        //[HttpPost]
        //public IActionResult AddProducts(int id, [Bind("DishId,Name,Price,CategoryId")] Dish dish, IFormCollection form)
        //{
        //    var selectedProd = _context.Dishes.Include(x => x.DishIngredients).FirstOrDefault(x => x.DishId == id);

        //    foreach (var ingre in selectedProd.DishIngredients)
        //    {
        //        _context.Remove(ingre);
        //    }
        //    _context.SaveChanges();

        //    foreach (var i in _ingredientService.All())
        //    {
        //        var dishIngredient = new DishIngredient()
        //        {
        //            Ingredient = i,
        //            Dish = selectedProd,
        //            Enabled = form.Keys.Any(x => x == $"ingredient-{i.IngredientId}")

        //        };
        //        _context.DishIngredients.Add(dishIngredient);
        //    }

        //    _context.SaveChangesAsync();

        //    var session = HttpContext.Session;

        //    Cart order;

        //    if (session.GetString("Dish") == null)
        //    {
        //        order = new Cart { DishCart = new List<DishCart>() };
        //    }
        //    else
        //    {
        //        var temp = session.GetString("Dish");
        //        order = JsonConvert.DeserializeObject<Cart>(temp);
        //    }

        //    DishCart dishCart = new DishCart
        //    {
        //        Quantity = 1,
        //        Dish = selectedProd,
        //        DishId = selectedProd.DishId
        //    };

        //    //if (order.DishCart.Any(x => x.DishId == selectedProd.DishId))
        //    //{
        //    //    order.DishCart.First(c => c.DishId == selectedProd.DishId).Quantity++;
        //    //}
        //    //else
        //    //{
        //    order.DishCart.Add(dishCart);
        //    //}



        //    var serializedValue = JsonConvert.SerializeObject(order, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        //    session.SetString("Dish", serializedValue);

        //    return PartialView("_OrderSummary", order.DishCart);
        //}


        public IActionResult AddDishToCart(int id, IFormCollection form)
        {
            var session = HttpContext.Session;

            var selectedProd = _context.Dishes.SingleOrDefault(x => x.DishId == id);

            Cart cart;

            if (session.GetString("Dish") == null)
            {
                cart = new Cart { DishCart = new List<DishCart>() };
            }
            else
            {
                var temp = session.GetString("Dish");
                cart = JsonConvert.DeserializeObject<Cart>(temp);
            }

            DishCart dishCart = new DishCart
            {
                Quantity = 1,
                Dish = selectedProd,
                DishId = selectedProd.DishId,
                Ingredient = selectedProd.Ingredient
            };

            //if (order.DishCart.Any(x => x.DishId == selectedProd.DishId))
            //{
            //    order.DishCart.First(c => c.DishId == selectedProd.DishId).Quantity++;
            //}
            //else
            //{
            cart.DishCart.Add(dishCart);
            //}

            var serializedValue = JsonConvert.SerializeObject(cart);
            session.SetString("Dish", serializedValue);

            return PartialView("_OrderSummary", cart.DishCart);
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
