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

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void GetCurrentDishorder(int id, HttpContext ctx)
        {

        }
    }
}
