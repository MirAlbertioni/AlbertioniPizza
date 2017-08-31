using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InmemDb.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using InmemDb.Models;
using InmemDb.Models.AccountViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InmemDb.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GuestPayment()
        {
            return View();
        }

        public IActionResult Payment()
        {

            return View();
        }
    }
}
