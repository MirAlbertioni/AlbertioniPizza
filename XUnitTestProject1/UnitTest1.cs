using InmemDb.Data;
using InmemDb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitTest1()
        {
            var efServiceprovider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(b => b
            .UseInMemoryDatabase("InmemDb")
            .UseInternalServiceProvider(efServiceprovider));
            services.AddTransient<IngredientService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void All_Are_Sorted()
        {
            var _ingredients = _serviceProvider.GetService<IngredientService>();
            var ings = _ingredients.All();

            Assert.Equal(ings.Count, 0);
        }
    }
}
