using InmemDb.Data;
using InmemDb.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InmemDb.Services
{
    public class IngredientService
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
