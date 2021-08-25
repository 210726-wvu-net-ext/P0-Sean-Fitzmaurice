using Models;
using System.Collections.Generic;

namespace BL
{
    /// <summary>
    /// Interface for BL
    /// </summary>
    public interface IRestaurantBL
    {
        List<Customer> SearchCustomers(string name);
        Customer GetCustomer(string name);
        Customer AddCustomer(Customer customer);
        Restaurant AddRestaurant(Restaurant restaurant);
        Review LeaveReview(Review review);
        decimal AverageRating(Restaurant restaurant);
        List<Review> FindRatingsByRestaurantId(Restaurant restaurant);
        List<Review> FindReviewsByCustomer(Customer customer);
        Customer GetCustomerById(int Id);
        List<Restaurant> SearchRestaurantsName(string name);
        List<Restaurant> SearchRestaurantsAddress(string address);
        List<Restaurant> SearchRestaurantsZip(int zip);
        Review GetReviewById(int Id);
        void DeleteReview(Models.Review review);
        void DeleteUser(Models.Customer customer);
        void DeleteRestaurant(Models.Restaurant restaurant);
    }
}