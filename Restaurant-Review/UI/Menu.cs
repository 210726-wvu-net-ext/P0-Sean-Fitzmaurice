using System;
using BL;
using Models;
using System.Collections.Generic;

namespace UI
{
    public class MainMenu : IMenu
    {
        public IRestaurantBL _restaurantBL;
        public Customer CurrentCustomer; //current user is set after a successful login, used to track customer Id on reviews left
        public MainMenu(IRestaurantBL bl)
        {
            _restaurantBL = bl;
            
        }

        public void Start()
        {
            
            bool isAdmin = false;
            bool repeat = true;
            bool exit = false;
            //Login Loop
            while(repeat){
                Console.WriteLine("\n*********************************************");
                Console.WriteLine("Welcome to Restuarant Reviews");
                Console.WriteLine("[0] Exit");
                Console.WriteLine("[1] New User");
                Console.WriteLine("[2] Existing User Login");
                Console.WriteLine("[3] Admin Login");
                
                switch(Console.ReadLine())
                {
                    case "0":
                        Console.WriteLine("Exiting");
                        repeat = false;
                        exit = true;
                    break;

                    case "1":
                        AddCustomer();
                    break;
                        
                    case "2":
                        repeat = Login();
                    break;

                    case "3":
                        repeat = Login();
                        if(this.CurrentCustomer.Admin == null)
                        {
                            repeat = true;
                            Console.WriteLine("Not an admin! Sign in as a regular customer using 2");
                        }else
                        {
                            isAdmin = true;
                        }
                        
                    break;

                    default:
                        Console.WriteLine("Not a valid command");
                        Console.WriteLine("*********************************************");
                    break;
                }

            }

            //Admin Functionality
            if(isAdmin){
                while(!exit){
                    Console.WriteLine("*********************************************");
                    Console.WriteLine("Admin Access: ");
                    Console.WriteLine("[0] Exit");
                    Console.WriteLine("[1] User Search");
                    switch(Console.ReadLine())
                    {
                        case "0":
                            Console.WriteLine("Exiting");
                            exit = true;
                        break;

                        case "1":
                            string name = Console.ReadLine();
                            Customer searchedCustomer = AdminCustomerSearch(name);
                            if(searchedCustomer.Name is null)
                            {
                                Console.WriteLine($"{name}Not a username");
                            }else
                            {
                                Console.WriteLine($"{searchedCustomer.Name}: \n Phone # {searchedCustomer.Phone} \n Email {searchedCustomer.Email}");

                            }
                        break;

                        default:
                            Console.WriteLine("Not a valid command");
                            Console.WriteLine("*********************************************");
                        break;
                    }
                }
            }else{
                //User functionality
                while(!exit){
                    Console.WriteLine("\n*********************************************");
                    Console.WriteLine("[0] Exit");
                    Console.WriteLine("[1] Add a restaurant");
                    Console.WriteLine("[2] Search Restaurants by name");
                    Console.WriteLine("[3] Search Restaurants by address");
                    Console.WriteLine("[4] Search Restaurants by zip");
                    Console.WriteLine("[5] ");
                    Restaurant foundRestaurant;
                    switch(Console.ReadLine())
                    {
                        case "0":
                            Console.WriteLine("Exiting");
                            exit = true;
                        break;

                        case "1":
                            AddRestaurant();
                        break;

                        case "2":
                            foundRestaurant = SearchRestaurantsName();
                            restaurantMenu(foundRestaurant);
                        break;

                        case "3":
                            //foundRestaurant = SearchRestaurantsZip();
                        break;

                        case "4":
                            //foundRestaurant = SearchRestaurantsAdd();
                        break;

                        default:
                            Console.WriteLine("Not a valid command");
                            Console.WriteLine("*********************************************");
                        break;
                    }
                }
            }
            

            //search for restaunt

            //options to see reviews and leave reviews

        }


        //function to ensure null or whitespace values are not entered by user, Parameter message is the message which prompts input from the user
        public string ValidInput(string message)
        {
            
            string input;
            do
            {
                Console.WriteLine(message);
                input = Console.ReadLine();
                if(String.IsNullOrWhiteSpace(input)){
                    Console.WriteLine("\nEntry Cannot be blank\n");
                }
            } while (String.IsNullOrWhiteSpace(input));
            return input;
        }

        //Adds new review to database, Creates Review object and passes it on to BL, Parameter restaurant is restaurant associated with review
        public void LeaveReview(Restaurant restaurant)
        {
            string comment;
            decimal rating;
            bool valid = false;
            Review latestReview;
            Console.WriteLine("How many stars would you give this restaurant? (out of 5)");
            do
            {
                if(decimal.TryParse(Console.ReadLine(), out rating))
                {
                    valid = true;
                }
            }while(!valid);
            Console.WriteLine("Leave a comment!");
            comment = Console.ReadLine();
            if(String.IsNullOrWhiteSpace(comment))
            {
                latestReview = new Review(rating, this.CurrentCustomer.Id, restaurant.Id);
            }else
            {
                latestReview = new Review(rating, this.CurrentCustomer.Id, restaurant.Id, comment);
            }
            _restaurantBL.LeaveReview(latestReview);
            Console.WriteLine($"Successfully left a review for {restaurant.Name}");
        }
        
        //Search Restaurants by name, returns a restaurant object mapped to values in database
        //TODO: in place of found {name} display information on restaurant
        public Restaurant SearchRestaurantsName()
        {
            string name = ValidInput("Enter name of Restaurant");
            Restaurant foundRestaurant = _restaurantBL.SearchRestaurantsName(name);
            if(foundRestaurant.Name is null){
                Console.WriteLine($"Restaurant: {name} was not found, please try seaching again, or add a new restaurant with 2");
            }else
            {
                Console.WriteLine($"Found {name}");
            }
            return foundRestaurant;
        }
        
        /*public Restaurant SearchRestaurantsZip()
        {

        }
        
        public Restaurant SearchRestaurantsAdd()
        {

        }*/

        //Adds a new restaurant to the database by creating restaurant object and passing it on to BL
        public void AddRestaurant(){
            string name;
            string address;
            int zip;
            Restaurant restaurantToAdd;
            name = ValidInput("Enter Restaurants Name");
            //add input validation for address and zip
            address = ValidInput("Enter Address");
            zip = 12538;
            restaurantToAdd = new Restaurant(name, address, zip);
            restaurantToAdd = _restaurantBL.AddRestaurant(restaurantToAdd);
        }

        //Adds a new customer to database database by creating restaurant object and passing it on to BL
        public void AddCustomer(){
            string name;
            string pass;
            string phone;
            string email;
            Customer latestCustomer;
            name = ValidInput("Enter Name: ");
            pass = ValidInput("Enter password: ");
            phone = ValidInput("Enter phone number: ");
            email = ValidInput("Enter email: ");
            
            latestCustomer = new Customer(name, pass, email, phone, null);
            latestCustomer = _restaurantBL.AddCustomer(latestCustomer);

            this.CurrentCustomer = latestCustomer;
            Console.WriteLine($"You may now log in as {name}");
        }

        //RETURNS TRUE IF LOGIN FAILS SO LOGIN LOOP CONTINUES
        public bool Login()
        {
            string name;
            string pass;
            name = ValidInput("Enter username: ");
            
            this.CurrentCustomer = _restaurantBL.SearchCustomers(name);

            if(this.CurrentCustomer.Name is null){
                Console.WriteLine($"{name} is not a username");
            }else
            {
                Console.WriteLine("Enter Password: ");
                pass = Console.ReadLine();
                if(pass == this.CurrentCustomer.Pass){
                    return false;
                }
                Console.WriteLine("Incorrect Password");
            }
            return true;
        }

        //Admin search of a customer
        //TODO display customer information and latest review
        public Customer AdminCustomerSearch(string name)
        {


            Customer foundCustomer = _restaurantBL.SearchCustomers(name);
            return foundCustomer;
        }
        //Menu for once you find restaurant
        public void restaurantMenu(Restaurant restaurant){
            bool repeat = true;
            Console.WriteLine("\n*********************************************");
            Console.WriteLine($"{restaurant.Name}\n{restaurant.Address} {restaurant.Zip}");
            Console.WriteLine($"Average rating: {decimal.Round(_restaurantBL.AverageRating(restaurant),2)}/5 stars");
            Console.WriteLine("Recent Reviews");
            //2 or 3 recent reviews
            do{
                Console.WriteLine("\n*********************************************");
                Console.WriteLine("[1] Leave Review for this restaurant");
                Console.WriteLine("[2] See all reviews for this restaurant");
                Console.WriteLine("[3] Find another restaurant (go to previous menu)");
                switch(Console.ReadLine()){
                    case "1":
                        LeaveReview(restaurant);
                    break;

                    case "2":

                    break;

                    case "3":
                        repeat = false;
                    break;

                    default:

                    break;

                }

            }while(repeat);
        }   

    }
}