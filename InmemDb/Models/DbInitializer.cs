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
            var aUser = new ApplicationUser
            {
                UserName = "student@test.com",
                Email = "student@test.com"
            };
            var r = userManager.CreateAsync(aUser, "Pa$$w0rd").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser
            {
                UserName = "admin@test.com",
                Email = "admin@test.com"
            };
            var adminuserResult = userManager.CreateAsync(adminUser, "Pa$$w0rd").Result;

            userManager.AddToRoleAsync(adminUser, "Admin");

            

            if(context.Dishes.ToList().Count == 0)
            {
                var pizza = new Category { Name = "Pizza" };
                var pasta = new Category { Name = "Pasta" };
                var salad = new Category { Name = "Salad" };
                

                //Ingredients
                var cheese = new Ingredient { Name = "Cheese" };
                var tomatoe = new Ingredient { Name = "Tomatoe" };
                var tomatoSauce = new Ingredient { Name = "Tomato sauce" };
                var ham = new Ingredient { Name = "Ham" };
                var tuna = new Ingredient { Name = "Tuna" };
                var olives = new Ingredient { Name = "Olives" };
                var pepperoni = new Ingredient { Name = "Pepperoni" };
                var kebab = new Ingredient { Name = "Kebab" };
                var pickles = new Ingredient { Name = "Pickles" };
                var broccoli = new Ingredient { Name = "Broccoli" };
                var garlic = new Ingredient { Name = "Garlic" };
                var cuecumber = new Ingredient { Name = "Cuecumber" };
                var onion = new Ingredient { Name = "Onion" };
                var letuce = new Ingredient { Name = "Letuce" };

                //Pizza
                var pCappricciosa = new Dish { Name = "Cappricciosa", Price = 79, Category =  pizza};
                var pCappricciosaCheese = new DishIngredient { Dish = pCappricciosa, Ingredient = cheese };
                var pCappricciosaTomatoe = new DishIngredient { Dish = pCappricciosa, Ingredient = tomatoe };
                var pCappricciosaHam = new DishIngredient { Dish = pCappricciosa, Ingredient = ham };
                pCappricciosa.DishIngredients = new List<DishIngredient>
                {
                    pCappricciosaCheese,
                    pCappricciosaTomatoe,
                    pCappricciosaHam
                };

                var pMargaritha = new Dish { Name = "Margaritha", Price = 69, Category = pizza };
                var pMargarithaCheese = new DishIngredient { Dish = pMargaritha, Ingredient = cheese };
                var pMargarithaTomatoSauce = new DishIngredient { Dish = pMargaritha, Ingredient = tomatoSauce };
                pMargaritha.DishIngredients = new List<DishIngredient>
                {
                    pMargarithaCheese,
                    pMargarithaTomatoSauce
                };

                var pHawaii = new Dish { Name = "Hawaii", Price = 85, Category = pizza };
                var pHawaiiCheese = new DishIngredient { Dish = pHawaii, Ingredient = cheese };
                var pHawaiiTomatoSauce = new DishIngredient { Dish = pHawaii, Ingredient = tomatoSauce };
                var pHawaiiOlives = new DishIngredient { Dish = pHawaii, Ingredient = olives };
                var pHawaiiPickles = new DishIngredient { Dish = pHawaii, Ingredient = pickles };
                var pHawaiiPepperoni = new DishIngredient { Dish = pHawaii, Ingredient = pepperoni };
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
                var pBologneseTomatoSauce = new DishIngredient { Dish = pBolognese, Ingredient = tomatoSauce };
                var pBologneseHam = new DishIngredient { Dish = pBolognese, Ingredient = ham };
                var pBologneseGarlic = new DishIngredient { Dish = pBolognese, Ingredient = garlic };
                pBolognese.DishIngredients = new List<DishIngredient>
                {
                    pBologneseTomatoSauce,
                    pBologneseHam,
                    pBologneseGarlic
                };

                //Salad
                var sGreece = new Dish { Name = "Greece Salad", Price = 80, Category = salad };
                var sGreeceOlives = new DishIngredient { Dish = sGreece, Ingredient = olives };
                var sGreeceCheese = new DishIngredient { Dish = sGreece, Ingredient = cheese };
                var sGreeceGarlic = new DishIngredient { Dish = sGreece, Ingredient = garlic };
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
    }
}
