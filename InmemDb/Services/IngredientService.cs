using InmemDb.Data;
using InmemDb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            var ingredients = _context.DishIngredients.Include(di => di.Ingredient).Where(di => di.DishId == id);
            string allIngredients = "";
            foreach (var ing in ingredients)
            {
                allIngredients += ing.Ingredient.Name + " ";
            }
            return allIngredients;
        }
    }
}
