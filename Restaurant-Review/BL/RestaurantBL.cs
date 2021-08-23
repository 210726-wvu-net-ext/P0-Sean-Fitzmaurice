using Models;
using DL;
using System.Collections.Generic;

namespace BL
{
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

        public Restaurant SearchRestaurantsName(string name)
        {
            return _repo.SearchRestaurantsName(name);
        }

        public Customer SearchCustomers(string name)
        {
            return _repo.SearchCustomers(name);
        }

        public Review LeaveReview(Review review){
            return _repo.LeaveReview(review);
        }
    }
}