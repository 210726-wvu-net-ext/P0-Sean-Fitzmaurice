using Models;
using DL.Entities;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DL
{
    public class RestaurantRepo : IRestaurantRepo
    {
        private RestaurantReviewContext _context;
        public RestaurantRepo(RestaurantReviewContext context)
        {
            _context = context;
        }

        public Models.Customer AddCustomer(Models.Customer customer)
        {
            _context.Customers.Add(
                new Entities.Customer{
                    Name = customer.Name,
                    Pass = customer.Pass,
                    Phone = customer.Phone,
                    Email = customer.Email,
                    IsAdmin = customer.Admin
                }
            );
            _context.SaveChanges();

            return customer;
        }
        public Models.Restaurant AddRestaurant(Models.Restaurant restaurant)
        {
            _context.Restaurants.Add(
                new Entities.Restaurant{
                    Name = restaurant.Name,
                    Address = restaurant.Address,
                    Zip = restaurant.Zip
                }
            );
            _context.SaveChanges();

            return restaurant;
        }
        
        public Models.Restaurant SearchRestaurantsName(string name)
        {
            Entities.Restaurant foundRestaurant =  _context.Restaurants
                .FirstOrDefault(Restaurant => Restaurant.Name == name);
            if(foundRestaurant != null)
            {
                return new Models.Restaurant(foundRestaurant.Id, foundRestaurant.Name, foundRestaurant.Address,foundRestaurant.Zip);
            }
            return new Models.Restaurant();
        }

        public Models.Customer SearchCustomers(string name)
        {
            Entities.Customer foundCustomer =  _context.Customers
                .FirstOrDefault(customer => customer.Name == name);
            if(foundCustomer!= null)
            {
                return new Models.Customer(foundCustomer.Id, foundCustomer.Name, foundCustomer.Pass, foundCustomer.Phone, foundCustomer.Email, foundCustomer.IsAdmin);
            }
            return new Models.Customer();
        }

        public Models.Review LeaveReview(Models.Review review)
        {
            _context.Reviews.Add(
                new Entities.Review{
                    CustomerId = review.CustomerId,
                    RestaurantId = review.RestaurantId,
                    Comment = review.textReview,
                    Stars = review.Stars
                }
            );
            _context.SaveChanges();

            return review;
        }
        
    }
}