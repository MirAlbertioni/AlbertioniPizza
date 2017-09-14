using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InmemDb.Data;
using InmemDb.Models;
using Microsoft.AspNetCore.Http;
using InmemDb.Services;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace InmemDb.Controllers
{
    public class DishController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DishController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index(int id)
        {
            var catlist = await _context.Category.ToListAsync();

            var dishes = await _context.Dishes
                .Include(x => x.DishIngredients)
                .ToListAsync();

            return View("Index", dishes);
        }

        // GET: Dishes/Create
        public async Task<IActionResult> Create(int dishId)
        {
            var model = new NewDishVM
            {
                Categories = await _context.Category.ToListAsync(),
                Ingredients = await _context.Ingredients.ToListAsync()
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int? id)
        {
            var model = new NewDishVM()
            {
                Dish = _context.Dishes.FirstOrDefault(x => x.DishId == id),
                Ingredients = _context.Ingredients.ToList(),
                IngredientIds = _context.DishIngredients
                .Include(x => x.Dish)
                .Include(x => x.Ingredient)
                .Where(s => s.DishId == id)
                .Select(i => i.IngredientId).ToList(),
                Categories = _context.Category.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NewDishVM newDish)
        {
            if (newDish.Dish.DishId == 0)
            {
                var dish = new Dish
                {
                    Name = newDish.Dish.Name,
                    CategoryId = newDish.Dish.CategoryId,
                    Price = newDish.Dish.Price
                };

                var ingredients = _context.Ingredients.Where(x => newDish.IngredientIds.Contains(x.IngredientId)).ToList();

                _context.Dishes.Add(dish);

                foreach (var item in ingredients)
                {
                    var dishIngredient = new DishIngredient
                    {
                        Dish = dish,
                        Ingredient = item
                    };
                    _context.DishIngredients.Add(dishIngredient);
                }


                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                var dish = _context.Dishes
                    .Include(x => x.DishIngredients)
                    .ThenInclude(i => i.Ingredient)
                    .SingleOrDefault(s => s.DishId == newDish.Dish.DishId);


                var ingredientsToRemove = _context.DishIngredients
                .Where(x => x.DishId == newDish.Dish.DishId).ToList();

                foreach (var item in ingredientsToRemove)
                {
                    dish.DishIngredients.Remove(item);
                }
                _context.SaveChanges();
                var dishIngredients = _context.Ingredients.Where(x => newDish.IngredientIds.Contains(x.IngredientId)).ToList();

                var listOfDishIngredient = new List<DishIngredient>();

                foreach (var item in dishIngredients)
                {
                    var x = new DishIngredient
                    {
                        Dish = dish,
                        DishId = dish.DishId,
                        Ingredient = item,
                        IngredientId = item.IngredientId
                    };
                    _context.DishIngredients.Add(x);
                }

                dish.Name = newDish.Dish.Name;
                dish.Price = newDish.Dish.Price;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .SingleOrDefaultAsync(m => m.DishId == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.DishId == id);
        }
    }
}
