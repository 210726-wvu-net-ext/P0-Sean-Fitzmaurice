using System;

namespace Models{
    public class Review {
        public Review(){}

        public Review(int reviewId, decimal stars, int customer, int restaurant, string textReview) {
            this.Id = reviewId;
            this.Stars = stars; 
            this.textReview = textReview;
            this.CustomerId = customer;
            this.RestaurantId = restaurant;
        }

        public Review(decimal stars, int customer, int restaurant, string textReview) {
            this.Stars = stars; 
            this.textReview = textReview;
            this.CustomerId = customer;
            this.RestaurantId = restaurant;
        }
        public Review(decimal stars, int customer, int restaurant){
            this.Stars = stars; 
            this.CustomerId = customer;
            this.RestaurantId = restaurant;
        }

        public int Id{ get; set; }
        public int CustomerId{ get; set; }
        public int RestaurantId{ get; set; }
        public decimal Stars { get; set;}
        public string textReview { get; set; }

    }
}