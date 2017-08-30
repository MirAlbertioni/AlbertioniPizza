using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InmemDb.Data;
using InmemDb.Models;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using InmemDb.Services;

namespace InmemDb.Controllers
{
    public class DishesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IngredientService _ingredientService;

        public DishesController(ApplicationDbContext context, IngredientService ingredientService)
        {
            _context = context;
            _ingredientService = ingredientService;
        }

        // GET: Dishes
        public async Task<IActionResult> Index(int? id)
        {
            var catlist = _context.Category.ToList();

            var dish = _context.Dishes
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient);

            return View(await _context.Dishes.ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes
                .Include(d => d.DishIngredients)
                .ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.DishId == id);

            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create(int? id, Dish dish)
        {
            ViewData["categoryList"] = new SelectList(_context.Category, "CategoryId", "Name", dish.CategoryId);

            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishId,Name,Price,CategoryId")] Dish dish, IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                var addIngredients = _ingredientService.All();

                foreach (var ingredient in addIngredients)
                {
                    var dishIngredient = new DishIngredient
                    {
                        Ingredient = ingredient,
                        Dish = dish,
                        Enabled = form.Keys.Any(x => x == $"ingredient-{ingredient.IngredientId}")
                    };
                    _context.DishIngredients.Add(dishIngredient);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.SingleOrDefaultAsync(m => m.DishId == id);
            
            ViewData["categoryList"] = new SelectList(_context.Category, "CategoryId", "Name", dish.CategoryId);

            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishId,Name,Price,CategoryId")] Dish dish, IFormCollection form)
        {
            if (id != dish.DishId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var addIngredients = _ingredientService.All();

                foreach (var ingredient in addIngredients)
                {
                    var dishIngredient = new DishIngredient
                    {
                        Ingredient = ingredient,
                        Dish = dish,
                        Enabled = form.Keys.Any(x => x == $"ingredient-{ingredient.IngredientId}")
                    };
                    _context.DishIngredients.Update(dishIngredient);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dish);
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

        public IActionResult Cart()
        {


            return View();
        }
    }
}
