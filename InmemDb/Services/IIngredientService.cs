using InmemDb.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmemDb.Services
{
    public interface IIngredientService
    {
        Task<List<Ingredient>> GetIngredients(int dishId);
        //Task<List<CartItemIngredient>> GetCartIngredients(int dishId);
    }
}
