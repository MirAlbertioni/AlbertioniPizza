using InmemDb.Data;
using InmemDb.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Newtonsoft.Json;

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
    }
}
