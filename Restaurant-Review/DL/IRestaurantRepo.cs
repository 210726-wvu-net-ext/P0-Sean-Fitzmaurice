using System;
using Models;
using System.Collections.Generic;

namespace DL
{
    public interface IRestaurantRepo
    {
        
        Restaurant AddRestaurant(Restaurant restaurant);

        Customer AddCustomer(Customer user);

        Customer GetCustomer(string name);

        List<Customer> SearchCustomers(string name);

        Review LeaveReview(Review review);

        List<Review> FindRatingsByRestaurantId(Restaurant restaurant);

        List<Review> FindReviewsByCustomer(Customer customer);
        
        Customer GetCustomerById(int Id);

        List<Restaurant> SearchRestaurantsName(string name);

        List<Restaurant> SearchRestaurantsAddress(string Address);

        List<Restaurant> SearchRestaurantsZip(int zip);

        void DeleteReview(Models.Review review);

        void DeleteUser(Models.Customer customer);

        void DeleteRestaurant(Models.Restaurant restaurant);
        
        Review GetReviewById(int Id);
        
    }
}
