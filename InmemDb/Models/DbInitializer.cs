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
            CreateUser(userManager, roleManager);
            CreateAdmin(userManager, roleManager);

            if (context.Dishes.ToList().Count == 0)
            {
                var pizza = new Category { Name = "Pizza" };
                var pasta = new Category { Name = "Pasta" };
                var salad = new Category { Name = "Salad" };


                //Ingredients
                var cheese = new Ingredient { Name = "Cheese", Price = 5 };
                var tomatoe = new Ingredient { Name = "Tomatoe", Price = 5 };
                var tomatoSauce = new Ingredient { Name = "Tomato sauce", Price = 5 };
                var ham = new Ingredient { Name = "Ham", Price = 7 };
                var tuna = new Ingredient { Name = "Tuna", Price = 6 };
                var olives = new Ingredient { Name = "Olives", Price = 5 };
                var pepperoni = new Ingredient { Name = "Pepperoni", Price = 5 };
                var kebab = new Ingredient { Name = "Kebab", Price = 7 };
                var pickles = new Ingredient { Name = "Pickles", Price = 4 };
                var broccoli = new Ingredient { Name = "Broccoli", Price = 5 };
                var garlic = new Ingredient { Name = "Garlic", Price = 4 };
                var cuecumber = new Ingredient { Name = "Cuecumber", Price = 5 };
                var onion = new Ingredient { Name = "Onion", Price = 3 };
                var letuce = new Ingredient { Name = "Letuce", Price = 3 };

                //Pizza
                var pCappricciosa = new Dish { Name = "Cappricciosa", Price = 79, Category = pizza };
                var pCappricciosaCheese = new DishIngredient { Dish = pCappricciosa, Ingredient = cheese, Enabled = true };
                var pCappricciosaTomatoe = new DishIngredient { Dish = pCappricciosa, Ingredient = tomatoe, Enabled = true };
                var pCappricciosaHam = new DishIngredient { Dish = pCappricciosa, Ingredient = ham, Enabled = true };
                pCappricciosa.DishIngredients = new List<DishIngredient>
                {
                    pCappricciosaCheese,
                    pCappricciosaTomatoe,
                    pCappricciosaHam
                };

                var pMargaritha = new Dish { Name = "Margaritha", Price = 69, Category = pizza };
                var pMargarithaCheese = new DishIngredient { Dish = pMargaritha, Ingredient = cheese, Enabled = true };
                var pMargarithaTomatoSauce = new DishIngredient { Dish = pMargaritha, Ingredient = tomatoSauce, Enabled = true };
                pMargaritha.DishIngredients = new List<DishIngredient>
                {
                    pMargarithaCheese,
                    pMargarithaTomatoSauce
                };

                var pHawaii = new Dish { Name = "Hawaii", Price = 85, Category = pizza };
                var pHawaiiCheese = new DishIngredient { Dish = pHawaii, Ingredient = cheese, Enabled = true };
                var pHawaiiTomatoSauce = new DishIngredient { Dish = pHawaii, Ingredient = tomatoSauce, Enabled = true };
                var pHawaiiOlives = new DishIngredient { Dish = pHawaii, Ingredient = olives, Enabled = true };
                var pHawaiiPickles = new DishIngredient { Dish = pHawaii, Ingredient = pickles, Enabled = true };
                var pHawaiiPepperoni = new DishIngredient { Dish = pHawaii, Ingredient = pepperoni, Enabled = true };
                pHawaii.DishIngredients = new List<DishIngredient>
                {
                    pHawaiiCheese,
                    pHawaiiTomatoSauce,
                    pHawaiiOlives,
                    pHawaiiPickles,
                    pHawaiiPepperoni
                };

                //Pasta
                var pBolognese = new Dish { Name = "Spaghetti Bolognese", Price = 75, Category = pasta };
                var pBologneseTomatoSauce = new DishIngredient { Dish = pBolognese, Ingredient = tomatoSauce, Enabled = true };
                var pBologneseHam = new DishIngredient { Dish = pBolognese, Ingredient = ham, Enabled = true };
                var pBologneseGarlic = new DishIngredient { Dish = pBolognese, Ingredient = garlic, Enabled = true };
                pBolognese.DishIngredients = new List<DishIngredient>
                {
                    pBologneseTomatoSauce,
                    pBologneseHam,
                    pBologneseGarlic
                };

                //Salad
                var sGreece = new Dish { Name = "Greece Salad", Price = 80, Category = salad };
                var sGreeceOlives = new DishIngredient { Dish = sGreece, Ingredient = olives, Enabled = true };
                var sGreeceCheese = new DishIngredient { Dish = sGreece, Ingredient = cheese, Enabled = true };
                var sGreeceGarlic = new DishIngredient { Dish = sGreece, Ingredient = garlic, Enabled = true };
                sGreece.DishIngredients = new List<DishIngredient>
                {
                    sGreeceOlives,
                    sGreeceCheese,
                    sGreeceGarlic
                };

                context.AddRange(cheese, tomatoe, tomatoSauce, ham, tuna, olives, pepperoni, kebab,
                    pickles, broccoli, garlic, cuecumber, onion, letuce);
                context.AddRange(pCappricciosa, pMargaritha, pHawaii, pBolognese, sGreece);
                context.AddRange(pizza, pasta, salad);
                context.SaveChanges();
            }
        }

        private static void CreateAdmin(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;
            var adminUser = new ApplicationUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com"
            };
            var adminuserResult = userManager.CreateAsync(adminUser, "Pa$$w0rd").Result;
            userManager.AddToRoleAsync(adminUser, "Admin");
        }

        private static void CreateUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var userRole = new IdentityRole { Name = "User" };
            var userResult = roleManager.CreateAsync(userRole).Result;
            var user = new ApplicationUser
            {
                UserName = "student@test.com",
                Email = "student@test.com",
                PhoneNumber = "1234567890",
                Address = "AbbasStreet 21",
                Zip = "12332",
                City = "Stockholm",
                Firstname = "Student",
                Lastname = "test"
            };
            var userUserResult = userManager.CreateAsync(user, "Pa$$w0rd").Result;
            userManager.AddToRoleAsync(user, "User");
        }
    }
}
