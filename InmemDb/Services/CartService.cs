using InmemDb.Data;
using InmemDb.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InmemDb.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CartItem> Cart(HttpContext httpContext)
        {

            Cart cart;
            List<CartItem> cartItems;
            var cartId = (int)httpContext.Session.GetInt32("Cart");

            cart = _context.Carts
                .Include(i => i.CartItem)
                .ThenInclude(x => x.CartItemIngredient)
                .ThenInclude(ig => ig.Ingredient)
                .Include(i => i.CartItem)
                .ThenInclude(ci => ci.Dish)
                .SingleOrDefault(x => x.CartId == cartId);

            cartItems = cart.CartItem;

            return cartItems;
        }

        public List<CartItem> AddDishToNewCart(int dishId, HttpContext httpContext)
        {
            int cartId;
            var dish = _context.Dishes.Include(x => x.DishIngredients)
                    .ThenInclude(i => i.Ingredient)
                    .SingleOrDefault(d => d.DishId == dishId);

            CartItem cartItem = new CartItem();
            List<CartItemIngredient> cartItemIngredient = new List<CartItemIngredient>();

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
            httpContext.Session.SetInt32("Cart", cartId);

            return newCart.CartItem;
        }

        public List<CartItem> AddDishToExistingCart(int dishId, HttpContext httpContext)
        {
            int cartId;
            var dish = _context.Dishes.Include(x => x.DishIngredients)
                    .ThenInclude(i => i.Ingredient)
                    .SingleOrDefault(d => d.DishId == dishId);

            CartItem cartItem = new CartItem();
            List<CartItemIngredient> cartItemIngredient = new List<CartItemIngredient>();

            cartId = (int)httpContext.Session.GetInt32("Cart");

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

            return currentCart.CartItem;
        }
    }
}
