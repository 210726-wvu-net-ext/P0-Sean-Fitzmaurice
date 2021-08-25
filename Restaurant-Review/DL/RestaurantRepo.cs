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

        public List<Models.Restaurant> SearchRestaurantsName(string name)
        {
            List<Models.Restaurant> list = _context.Restaurants.Select(
                Restaurant => new Models.Restaurant(Restaurant.Id, Restaurant.Name, Restaurant.Address, Restaurant.Zip)
            ).ToList();
            List<Models.Restaurant> query = list.Where(Restaurant => Restaurant.Name.ToLower().Contains(name.ToLower())).ToList();
            

            return query;
        }

        public List<Models.Restaurant> SearchRestaurantsAddress(string address)
        {
            List<Models.Restaurant> list = _context.Restaurants.Select(
                Restaurant => new Models.Restaurant(Restaurant.Id, Restaurant.Name, Restaurant.Address, Restaurant.Zip)
            ).ToList();
            List<Models.Restaurant> query = list.Where(Restaurant => Restaurant.Address.ToLower().Contains(address.ToLower())).ToList();
            

            return query;
        }

        public List<Models.Restaurant> SearchRestaurantsZip(int zip)
        {
            List<Models.Restaurant> list = _context.Restaurants.Select(
                Restaurant => new Models.Restaurant(Restaurant.Id, Restaurant.Name, Restaurant.Address, Restaurant.Zip)
            ).ToList();
            List<Models.Restaurant> query = list.Where(Restaurant => Restaurant.Zip == zip).ToList();
            

            return query;
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
        
        public List<Models.Review> FindRatingsByRestaurantId(Models.Restaurant restaurant){
            List<Models.Review> list = _context.Reviews.Select(
                Review => new Models.Review(Review.Id, Review.Stars, Review.CustomerId, Review.RestaurantId, Review.Comment)
            ).ToList();
            List<Models.Review> query = list.Where(Review => Review.RestaurantId == restaurant.Id).ToList();
            query.Reverse();
            return query;
            
        }

        public List<Models.Review> FindReviewsByCustomer(Models.Customer customer){
            
            List<Models.Review> list = _context.Reviews.Select(
                Review => new Models.Review(Review.Id, Review.Stars, Review.CustomerId, Review.RestaurantId, Review.Comment)
            ).ToList();
            List<Models.Review> query = list.Where(Review => Review.CustomerId == customer.Id).ToList();
            query.Reverse();
            return query;
        }
        
        

        public List<Models.Customer> SearchCustomers(string name)
        {
            List<Models.Customer> list = _context.Customers.Select(
                Customer => new Models.Customer(Customer.Id, Customer.Name, Customer.Pass, Customer.Phone, Customer.Email, Customer.IsAdmin)
            ).ToList();
            List<Models.Customer> query = list.Where(Customer => Customer.Name.Contains(name)).ToList();
            

            return query;
        }
        

        public Models.Customer GetCustomerById(int Id)
        {
            Entities.Customer foundCustomer = _context.Customers
                .FirstOrDefault(customer => customer.Id == Id);
            if(foundCustomer != null)
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

        public void DeleteUser(Models.Customer customer)
        {
            List<Models.Review> usersReviews = FindReviewsByCustomer(customer);
            foreach(Models.Review review in usersReviews){
                DeleteReview(review);
            }
            Entities.Customer userToDelete = new Entities.Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                Pass = customer.Pass,
                Phone = customer.Phone,
                Email = customer.Email,
                IsAdmin = customer.Admin
            };
            _context.Customers.Remove(userToDelete);
            _context.SaveChanges();
        }

        public void DeleteReview(Models.Review review)
        {
            Entities.Review reviewToDelete = new Entities.Review
            {
                Id = review.Id,
                CustomerId = review.CustomerId,
                RestaurantId = review.RestaurantId,
                Comment = review.textReview,
                Stars = review.Stars
            };
            _context.Reviews.Remove(reviewToDelete);
            _context.SaveChanges();
        }

        public void DeleteRestaurant(Models.Restaurant restaurant)
        {
            List<Models.Review> restaurantReviews = FindRatingsByRestaurantId(restaurant);
            foreach(Models.Review review in restaurantReviews){
                DeleteReview(review);
            }
            Entities.Restaurant restaurantToDelete = new Entities.Restaurant
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                Zip = restaurant.Zip
            };
            _context.Restaurants.Remove(restaurantToDelete);
            _context.SaveChanges();
        }

        public Models.Customer GetCustomer(string name){
            Entities.Customer foundCustomer =  _context.Customers
                .FirstOrDefault(customer => customer.Name == name);
            if(foundCustomer!= null)
            {
                return new Models.Customer(foundCustomer.Id, foundCustomer.Name, foundCustomer.Pass, foundCustomer.Phone, foundCustomer.Email, foundCustomer.IsAdmin);
            }
            return new Models.Customer();
        }
        public Models.Review GetReviewById(int Id){
            Entities.Review foundReview = _context.Reviews
                .FirstOrDefault(review => review.Id == Id);
            if( foundReview != null)
            {
            return new Models.Review(foundReview.Id, foundReview.Stars, foundReview.CustomerId, foundReview.RestaurantId, foundReview.Comment);
            }
            return new Models.Review();
        }
        
    }
}