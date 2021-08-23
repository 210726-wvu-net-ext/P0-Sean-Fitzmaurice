using System;
using BL;
using Models;
using System.Collections.Generic;

namespace UI
{
    public class MainMenu : IMenu
    {
        public IRestaurantBL _restaurantBL;
        public Customer CurrentCustomer;
        public MainMenu(IRestaurantBL bl)
        {
            _restaurantBL = bl;
            
        }

        public void Start()
        {
            //ask if admin
            bool isAdmin = false;
            bool repeat = true;
            bool exit = false;
            while(repeat){
                Console.WriteLine("*********************************************");
                Console.WriteLine("Welcome to Restuarant Reviews");
                Console.WriteLine("[0] Exit");
                Console.WriteLine("[1] New User");
                Console.WriteLine("[2] Existing User Login");
                Console.WriteLine("[3] Admin Login");
                //Sends login information to create user object and see if user object is in database
                switch(Console.ReadLine())
                {
                    case "0":
                        Console.WriteLine("Exiting");
                        repeat = false;
                        exit = true;
                    break;

                    case "1":
                        AddCustomer();
                        repeat = false;
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
                while(!exit){
                    Console.WriteLine("*********************************************");
                    Console.WriteLine("[0] Exit");
                    Console.WriteLine("[1] Review a restaurant");
                    Console.WriteLine("[2] Add new restaurant");
                    Console.WriteLine("[3] View a restaurant's details");
                    Console.WriteLine("[4] View a restaurant's reviews");
                    Console.WriteLine("[5] Search restaurants");
                    Restaurant foundRestaurant;
                    switch(Console.ReadLine())
                    {
                        case "0":
                            Console.WriteLine("Exiting");
                            exit = true;
                        break;

                        case "1":
                            
                        break;

                        case "2":
                            AddRestaurant();
                        break;

                        case "3":
                            foundRestaurant = SearchRestaurantsName();
                            if(foundRestaurant.Name != null)
                            {
                                Console.WriteLine("Would you like to leave a review for this restaurant?(Y for yes)");
                                string input = Console.ReadLine();
                                input.ToLower();
                                if(input.StartsWith("Y"))
                                {
                                    LeaveReview(foundRestaurant);
                                }
                            }
                        break;

                        case "4":
                            //foundRestaurant = SearchRestaurantsZip();
                        break;

                        case "5":
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



        public string ValidInput(string message)
        {
            Console.WriteLine(message);
            string input;
            do
            {
                input = Console.ReadLine();
            } while (String.IsNullOrWhiteSpace(input));
            return input;
        }

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
        public Customer AdminCustomerSearch(string name)
        {


            Customer foundCustomer = _restaurantBL.SearchCustomers(name);
            return foundCustomer;
        }

    }
}