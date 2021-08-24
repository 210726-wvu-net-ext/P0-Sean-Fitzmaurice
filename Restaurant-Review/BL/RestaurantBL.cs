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

        public decimal AverageRating(Restaurant restaurant){
            decimal avg;
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
            avg = tot/numReviews;
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
        public Customer GetCustomerById(int Id){
            return _repo.GetCustomerById(Id);
        }

    }
}