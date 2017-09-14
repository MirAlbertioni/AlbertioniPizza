using InmemDb.Data;
using InmemDb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly ApplicationDbContext _context;

        public IngredientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Ingredient> All()
        {
            return _context.Ingredients.ToList();
        }

        public async Task<List<Ingredient>> GetIngredients(int dishId)
        {
            return await _context.DishIngredients.Where(x => x.DishId == dishId).Select(i => i.Ingredient).ToListAsync();
        }

        //public async Task<List<CartItemIngredient>> GetCartIngredients(int dishId)
        //{
        //    return await _context.CartItems.Include(x => x.CartItemIngredient)
        //        .Where(x => x.Dish.DishId == dishId).Select(i => i.IngredientCartItem).ToListAsync();
        //}

        public string AllToStringForDishId(int id)
        {
            var ingredients = _context.DishIngredients.Include(di => di.Ingredient).Where(di => di.DishId == id && di.Enabled);
            string allIngredients = "";
            foreach (var ing in ingredients)
            {
                allIngredients += ing.Ingredient.Name + " ";
            }
            return allIngredients;
        }
    }
}
