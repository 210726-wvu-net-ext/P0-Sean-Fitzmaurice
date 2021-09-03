using Models;
using DL;
using System.Collections.Generic;
using System.IO;
using System;
using Serilog;

namespace BL
{
    /// <summary>
    /// BL handels translation between UI and DL layer. It performans additional cacluations on the data when returning from DL to UI. For example, calcuating the average rating of reviews for a restaurant. 
    /// </summary>
    public class RestaurantBL : IRestaurantBL
    {
        private IRestaurantRepo _repo;

        public RestaurantBL(IRestaurantRepo repo){
            _repo = repo;
        }
        
        public Customer AddCustomer(Customer customer)
        {
            return _repo.AddCustomer(customer);
        }

        public Restaurant AddRestaurant(Restaurant restaurant)
        {
            return _repo.AddRestaurant(restaurant);
        }

        public List<Customer> SearchCustomers(string name)
        {
            return _repo.SearchCustomers(name);
        }

        public Review LeaveReview(Review review){
            return _repo.LeaveReview(review);
        }
        /// <summary>
        /// calculates average rating (stars) of a restaurant
        /// </summary>
        /// <param name="restaurant">restaurant for reviews to be calculated</param>
        /// <returns>decimal of average rating</returns>
        public decimal AverageRating(Restaurant restaurant){
            decimal avg = 0;
            decimal numReviews = 0;
            decimal tot = 0;
            List<Review> reviews = _repo.FindRatingsByRestaurantId(restaurant);
            if(reviews.Count == 0){
                return -1;
            }
            foreach(Review review in reviews)
            {
                tot += review.Stars;
                numReviews += 1;
            }
            try{
                avg = tot/numReviews;
            }
            catch (ArithmeticException a)
            {
                Log.Fatal(a, "Program Exit");
            }
            return avg;
        }
        public List<Review> FindRatingsByRestaurantId(Restaurant restaurant)
        {
            return _repo.FindRatingsByRestaurantId(restaurant);
        }

        public List<Review> FindReviewsByCustomer(Customer customer)
        {
            return _repo.FindReviewsByCustomer(customer);
        }

        public Customer GetCustomerById(int Id)
        {
            return _repo.GetCustomerById(Id);
        }

        public List<Restaurant> SearchRestaurantsName(string name)
        {
            return _repo.SearchRestaurantsName(name);
        }

        public List<Restaurant> SearchRestaurantsAddress(string address)
        {
            return _repo.SearchRestaurantsAddress(address);
        }

        public List<Restaurant> SearchRestaurantsZip(int zip)
        {
            return _repo.SearchRestaurantsZip(zip);
        }

        public void DeleteReview(Models.Review review)
        {
            _repo.DeleteReview(review);
        }

        public void DeleteUser(Models.Customer customer)
        {
            _repo.DeleteUser(customer);
        }
        
        public void DeleteRestaurant(Models.Restaurant restaurant)
        {
            _repo.DeleteRestaurant(restaurant);
        }
        public Customer GetCustomer(string name){
            return _repo.GetCustomer(name);
        }

        public Review GetReviewById(int Id){
            return _repo.GetReviewById(Id);
        }
    }
}