using InmemDb.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Models
{
    public static class DbInitializer
    {
        public static void Initializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var aUser = new ApplicationUser();
            aUser.UserName = "student@test.com";
            aUser.Email = "student@test.com";
            var r = userManager.CreateAsync(aUser, "Pa$$w0rd").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser();
            adminUser.UserName = "admin@test.com";
            adminUser.Email = "admin@test.com";
            var adminuserResult = userManager.CreateAsync(adminUser, "Pa$$w0rd").Result;

            userManager.AddToRoleAsync(adminUser, "Admin");


            if(context.Dishes.ToList().Count == 0)
            {
                var cheese = new Ingridient { Name = "Cheese" };
                var tomatoe = new Ingridient { Name = "Tomatoe" };
                var ham = new Ingridient { Name = "Ham" };

                var cappricciosa = new Dish { Name = "Cappricciosa", Price = 79 };
                var margaritha = new Dish { Name = "Margaritha", Price = 69  };
                var hawaii = new Dish { Name = "Hawaii", Price = 85 };

                var cappricciosaCheese = new DishIngridient { Dish = cappricciosa, Ingridient = cheese };
                var cappricciosaTomatoe = new DishIngridient { Dish = cappricciosa, Ingridient = tomatoe };
                var cappricciosaHam = new DishIngridient { Dish = cappricciosa, Ingridient = ham };
                cappricciosa.DishIngridients = new List<DishIngridient>();
                cappricciosa.DishIngridients.Add(cappricciosaCheese);
                cappricciosa.DishIngridients.Add(cappricciosaTomatoe);
                cappricciosa.DishIngridients.Add(cappricciosaHam);
                context.AddRange(cheese, tomatoe, ham);

                context.AddRange(cappricciosa, margaritha, hawaii);
                context.SaveChanges();
            }
        }
    }
}
