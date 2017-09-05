using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InmemDb.Models;
using InmemDb.Data;
using Microsoft.EntityFrameworkCore;
using InmemDb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InmemDb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IngredientService _ingredientService;

        public HomeController(ApplicationDbContext context, IngredientService ingredientService)
        {
            _context = context;
            _ingredientService = ingredientService;
        }

        // GET: Dishes
        public async Task<IActionResult> Index(int? id)
        {
            var catlist = _context.Categories.ToList();

            return View(await _context.Dishes.ToListAsync());
        }

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var dish = await _context.Dishes.Include(x => x.DishIngredients).SingleOrDefaultAsync(m => m.DishId == id);

        //    if (dish == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(dish);
        //}

        //[HttpPost]
        //public IActionResult Edit(int id, [Bind("DishId,Name,Price,CategoryId")] Dish dish, IFormCollection form)
        //{
        //    var dishToEdit = _context.Dishes.Include(x => x.DishIngredients).FirstOrDefault(x => x.DishId == id);

        //    foreach (var ingre in dishToEdit.DishIngredients)
        //    {
        //        _context.Remove(ingre);
        //    }
        //    _context.SaveChanges();

        //    foreach (var i in _ingredientService.All())
        //    {
        //        var dishIngredient = new DishIngredient()
        //        {
        //            Ingredient = i,
        //            Dish = dishToEdit,
        //            Enabled = form.Keys.Any(x => x == $"ingredient-{i.IngredientId}")

        //        };
        //        _context.DishIngredients.Add(dishIngredient);
        //    }
        //    dishToEdit.Name = dish.Name;
        //    dishToEdit.Price = dish.Price;
        //    dishToEdit.CategoryId = dish.CategoryId;
        //    dishToEdit.Ingredient = dish.Ingredient;

        //    _context.SaveChangesAsync();

        //    return PartialView("_ExtraIngredient", dishToEdit);
        //}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
