using UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DL.Entities;
using BL;
using DL;
using System.IO;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            // logging
        
            var configuaration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // add connection try catch
        
        string connectionString = configuaration.GetConnectionString("RestaurantDB");
        DbContextOptions<RestaurantReviewContext> options = new DbContextOptionsBuilder<RestaurantReviewContext>()
            .UseSqlServer(connectionString)
            .Options;
        
        var context = new RestaurantReviewContext(options);
        IMenu menu = new MainMenu(new RestaurantBL(new RestaurantRepo(context)));

        //add try catch block
        menu.Start();


        }
    }
}
