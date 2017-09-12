using InmemDb.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Services
{
    public class DishService
    {
        private readonly ApplicationDbContext _context;
        private readonly IngredientService _ingredientService;

        public DishService(ApplicationDbContext context, IngredientService ingredientService)
        {
            _context = context;
            _ingredientService = ingredientService;
        }

        //public bool HasIngredient(int id)
        //{
        //    var dishes = _context.Dishes.Include(di => di.DishIngredients).SingleOrDefault(x => x.DishId == id);
        //    return dishes;
        //}
    }
}
