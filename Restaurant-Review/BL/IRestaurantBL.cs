using Models;
using System.Collections.Generic;

namespace BL
{
    public interface IRestaurantBL
    {
        Customer SearchCustomers(string name);
        Customer AddCustomer(Customer customer);
        Restaurant AddRestaurant(Restaurant restaurant);
        Review LeaveReview(Review review);
        Restaurant SearchRestaurantsName(string name);
        //Restaurant ViewDetials();
    }
}