using InmemDb.Data;
using InmemDb.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace InmemDb.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IngredientService _ingredientService;

        public CartService(ApplicationDbContext context, IngredientService ingredientService)
        {
            _context = context;
            _ingredientService = ingredientService;
        }

        //public ICollection<DishCart> AddDishToCart(int id)
        //{
        //    var selectedProd = _context.Dishes.Include(x => x.DishIngredients).ThenInclude(i => i.Ingredient).SingleOrDefault(d => d.DishId == id);
        //    Cart cart;
        //    cart = new Cart { DishCart = new List<DishCart>() };
            

        //    DishCart dishCart = new DishCart
        //    {
        //        Quantity = 1,
        //        Dish = selectedProd,
        //        DishId = selectedProd.DishId,
        //        Ingredient = selectedProd.Ingredient
        //    };

        //    _context.Cart.Add(dishCart.Cart);
        //    _context.SaveChanges();

        //    return cart.DishCart;
        //}

        //public ICollection<DishCart> AddToExcistingCart(int id)
        //{
        //    var session = HttpContext.Session;
        //    var selectedProd = _context.Dishes.SingleOrDefault(x => x.DishId == id);

           
            
        //        var temp = session.GetString("Cart");
        //        cart = JsonConvert.DeserializeObject<Cart>(temp);
            

        //    DishCart dishCart = new DishCart
        //    {
        //        Quantity = 1,
        //        Dish = selectedProd,
        //        DishId = selectedProd.DishId,
        //        Ingredient = selectedProd.Ingredient
        //    };

        //    cart.DishCart.Add(dishCart);

        //    var serializedValue = JsonConvert.SerializeObject(cart);
        //    session.SetString("Cart", serializedValue);
        //}

        //public ICollection<DishCart> AddDishToCart(int id, HttpContext httpContext)
        //{
        //    var session = httpContext.Session;

        //    var selectedProd = _context.Dishes.SingleOrDefault(x => x.DishId == id);

        //    Cart cart;

        //    if (session.GetString("Cart") == null)
        //    {
        //        cart = new Cart { DishCart = new List<DishCart>() };
        //    }
        //    else
        //    {
        //        var temp = session.GetString("Cart");
        //        cart = JsonConvert.DeserializeObject<Cart>(temp);
        //    }

        //    DishCart dishCart = new DishCart
        //    {
        //        Quantity = 1,
        //        Dish = selectedProd,
        //        DishId = selectedProd.DishId,
        //        Ingredient = selectedProd.Ingredient
        //    };

        //    cart.DishCart.Add(dishCart);

        //    var serializedValue = JsonConvert.SerializeObject(cart);
        //    session.SetString("Cart", serializedValue);

        //    return cart.DishCart;
        //}
    }
}
