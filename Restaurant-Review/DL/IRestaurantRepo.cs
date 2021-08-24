using System;
using Models;
using System.Collections.Generic;

namespace DL
{
    public interface IRestaurantRepo
    {
        
        Restaurant AddRestaurant(Restaurant restaurant);

        Customer AddCustomer(Customer user);

        Restaurant SearchRestaurantsName(string name);

        Customer SearchCustomers(string name);

        Review LeaveReview(Review review);

        List<Review> FindRatingsByRestaurantId(Restaurant restaurant);

        List<Review> FindReviewsByCustomer(Customer customer);
        
        Customer GetCustomerById(int Id);
        
    }
}
