using AnumalCoutingDatabase.API;
using AnumalCoutingDatabase.API.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AnumalCoutingDatabase.Tests
{
    public class DemoTest
    {
        [Fact]
        public void Test1()
        {
            Assert.True(1 == 1);
        }
        [Fact]
        public async Task CustomerInegrationTest()
        {
            //create Db context
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<CustomerContext>();
            optionsBuilder
                .UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
            var context = new CustomerContext(optionsBuilder.Options);

            //just to make sure: Drop existing db db_text and create existing db db_text
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            //create controller
            var controller = new CustomersController(context);

            //add customer
            await controller.Add(new Customer { CustomerName = "Khairy" });

            //check: Does getall return the added customer
            var result = (await controller.GetAll()).ToArray();
            Assert.Single(result);
            Assert.Equal("Khairy", result[0].CustomerName);
        }
    }
}
