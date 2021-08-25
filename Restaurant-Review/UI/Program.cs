using UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DL.Entities;
using BL;
using DL;
using System.IO;
using Serilog;
using System;

namespace UI
{
    class Program
    {
    /// <summary>
    /// Program class is used to run the main menu and set up connection to DB
    /// </summary>
        static void Main(string[] args)
        {
        
            try
            {
                var configuaration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                
                


                string connectionString = configuaration.GetConnectionString("RestaurantDB");
                DbContextOptions<RestaurantReviewContext> options = new DbContextOptionsBuilder<RestaurantReviewContext>()
                    .UseSqlServer(connectionString)
                    .Options;

                var context = new RestaurantReviewContext(options);
                IMenu menu = new MainMenu(new RestaurantBL(new RestaurantRepo(context)));

                try
                {
                    menu.Start();
                }
                catch(Exception e)
                {
                    Log.Fatal(e, "Program Exit");
                }
                }
            catch(Exception e)
            {
                Log.Fatal(e, "Program Exit");
            }
        }
    }
}
