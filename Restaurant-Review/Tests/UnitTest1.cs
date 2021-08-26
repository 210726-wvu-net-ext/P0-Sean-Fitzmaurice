using System;
using Xunit;
using BL;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void RestaurantTest()
        {
            Models.Restaurant testRestaurant = new Models.Restaurant(1, "Antonellas", "9196 Albany Post Road, Hyde Park, NY", 12538);
            string expected = "Antonellas";
            string actual = testRestaurant.Name;
            Assert.Equal(expected,actual);
            
        }

        [Fact]
        public void ReviewTest()
        {
            
         
            Models.Review testReview = new Models.Review(1, 2.5M,5 ,7, "It's ok I guess");
            int expected = 1;
            int actual = testReview.Id;
            Assert.Equal(expected,actual);
        }

    }
}
