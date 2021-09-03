using System;
using BL;
using Models;
using System.Collections.Generic;
using Serilog;

namespace UI
{
    /// <summary>
    /// Menu is the main class to run the program through, it passes users commands to the BL
    /// </summary>
    public class MainMenu : IMenu
    {
        public IRestaurantBL _restaurantBL;
        public Customer CurrentCustomer; //current user is set after a successful login, used to track customer Id on reviews left
        public MainMenu(IRestaurantBL bl)
        {
            _restaurantBL = bl;
            Log.Logger=new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .WriteTo.File("../logs/petlogs.txt", rollingInterval:RollingInterval.Day)
                            .CreateLogger();
            Log.Information("UI begining");
            
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
                            Console.WriteLine("\n***Not an admin! Sign in as a regular customer using 2***");
                        }else
                        {
                            isAdmin = true;
                        }
                        
                    break;

                    default:
                        Console.WriteLine("\n***Not a valid command***");
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
                    Console.WriteLine("[2] Delete Restaurant");
                    
                    switch(Console.ReadLine())
                    {
                        case "0":
                            Console.WriteLine("Exiting");
                            exit = true;
                        break;

                        case "1":
                            string name = ValidInput("Enter Customer name: ");
                            Customer searchedCustomer = CustomerListSelect(_restaurantBL.SearchCustomers(name));
                            AdminUserMenu(searchedCustomer);
                        break;

                        case "2":
                            Console.WriteLine("[0] Exit");
                            Console.WriteLine("[1] Search Restaurants by name");
                            Console.WriteLine("[2] Search Restaurants by address");
                            Console.WriteLine("[3] Search Restaurants by zip");
                            Restaurant restaurantToDelete;
                            switch(Console.ReadLine())
                            {

                                case "0":
                        
                                break;
                                case "1":
                                    restaurantToDelete = RestaurantListSelect(_restaurantBL.SearchRestaurantsName(ValidInput("Enter Restaurant name: ")));
                                    if(restaurantToDelete.Name != null){
                                        AdminRestaurantMenu(restaurantToDelete);
                                    }
                                    
                                break;

                                case "2":
                                    restaurantToDelete = RestaurantListSelect(_restaurantBL.SearchRestaurantsAddress(ValidInput("Enter Restaurant address/city: ")));
                                    if(restaurantToDelete != null){
                                        AdminRestaurantMenu(restaurantToDelete);
                                    }
                                break;

                                case "3":
                                bool valid = false;
                                int zip;
                                    do{
                                        Console.WriteLine("Enter Restaurant zip: ");
                                        if(int.TryParse(Console.ReadLine(), out zip))
                                        {
                                            valid = true;
                                        }else
                                        {
                                            Console.WriteLine("Not a valid zip!");
                                        }
                                    }while(!valid);

                                    
                                    restaurantToDelete = RestaurantListSelect(_restaurantBL.SearchRestaurantsZip(zip));
                                    if(restaurantToDelete.Name != null){
                                        AdminRestaurantMenu(restaurantToDelete);
                                    }
                                break;

                                default:
                                    Console.WriteLine("\n***Not a valid command***");
                                    Console.WriteLine("*********************************************");
                                break;
                            }
                        break;



                        default:
                            Console.WriteLine("\n***Not a valid command***");
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
                            foundRestaurant = RestaurantListSelect(_restaurantBL.SearchRestaurantsName(ValidInput("Enter Restaurant name: ")));
                            if(foundRestaurant.Name != null){
                                restaurantMenu(foundRestaurant);
                            }
                            
                        break;

                        case "3":
                            foundRestaurant = RestaurantListSelect(_restaurantBL.SearchRestaurantsAddress(ValidInput("Enter Restaurant address/city: ")));
                            if(foundRestaurant.Name != null){
                                restaurantMenu(foundRestaurant);
                            }
                        break;

                        case "4":
                        bool valid = false;
                        int zip;
                            do{
                                Console.WriteLine("Enter Restaurant zip: ");
                                if(int.TryParse(Console.ReadLine(), out zip))
                                {
                                    valid = true;
                                }else
                                {
                                    Console.WriteLine("Not a valid zip!");
                                }
                            }while(!valid);

                            
                            foundRestaurant = RestaurantListSelect(_restaurantBL.SearchRestaurantsZip(zip));
                            if(foundRestaurant.Name != null){
                                restaurantMenu(foundRestaurant);
                            }
                        break;

                        default:
                            Console.WriteLine("\n***Not a valid command***");
                            Console.WriteLine("*********************************************");
                        break;
                    }
                }
            }

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
                    Console.WriteLine("\n***Entry Cannot be blank***");
                }else if(input.Length > 100)
                {
                    Console.WriteLine("Entry cannot be longer than 100 characters");
                }

            } while (String.IsNullOrWhiteSpace(input));
            return input;
        }

        ///<summary>
        /// Select restaurant from variable length list, returns selected restaurant
        /// <param>
        /// List - list of restaurants to select from
        /// <param>
        /// <summary>
    
        public Restaurant RestaurantListSelect(List<Restaurant> list)
        {
            int len = list.Count;
            int count = 1;
            if (len == 1)
            {
                return list[0];
            }else if (len < 1)
            {
                Console.WriteLine("No matching results");
                return new Restaurant();
            }else
            {
                Console.WriteLine($"Found {len} matching results, please select");
                foreach(Restaurant restaurant in list)
                {
                    
                    Console.WriteLine("*********************************************");
                    Console.WriteLine($"Selection {count}: ");
                    Console.WriteLine($"{restaurant.Name}\n{restaurant.Address} {restaurant.Zip}");
                    Console.WriteLine("\n*********************************************");
                    count++;
                }
                bool valid = false;
                int select;
                do{
                    if(Int32.TryParse(Console.ReadLine(),out select))
                    {
                        if(select > 0 && select <= len){
                            valid = true;
                        }else
                        {
                            Console.WriteLine($"Not a valid selection, pick number between 1 and {len}");
                        }
                        
                        
                    }else
                    {
                        Console.WriteLine($"Not a valid selection, pick number between 1 and {len}");
                    }
                }while(!valid);
                select = select -1;
                Console.WriteLine("*********************************************");
                Console.WriteLine("*********************************************");
                try
                {
                    return list[select];
                }
                catch(IndexOutOfRangeException I)
                {
                    Log.Fatal(I, "Restaurant List select index was out of range");
                }
                return new Restaurant();
                
            }
        }

        //select customer from variable length list, returns selected customer
        public Customer CustomerListSelect(List<Customer> list)
        {
            int len = list.Count;
            int count = 1;
            if (len == 1)
            {
                return list[0];
            }else if (len < 1)
            {
                Console.WriteLine("No matching results");
                return new Customer();
            }else
            {
                Console.WriteLine($"Found {len} matching results, please select");
                foreach(Customer customer in list)
                {
                    
                    Console.WriteLine("*********************************************");
                    Console.WriteLine($"Selection {count}: ");
                    Console.WriteLine($"{customer.Name}\n{customer.Phone} {customer.Email}");
                    Console.WriteLine("\n*********************************************");
                    count++;
                }
                bool valid = false;
                int select;
                do{
                    if(Int32.TryParse(Console.ReadLine(),out select))
                    {
                        if(select > 0 && select <= len){
                            valid = true;
                        }else
                        {
                            Console.WriteLine($"Not a valid selection, pick number between 1 and {len}");
                        }
                        
                        
                    }else
                    {
                        Console.WriteLine($"Not a valid selection, pick number between 1 and {len}");
                    }
                }while(!valid);
                select = select -1;
                Console.WriteLine("*********************************************");
                Console.WriteLine("*********************************************");
                try
                {
                    return list[select];
                }
                catch(IndexOutOfRangeException I)
                {
                    Log.Fatal(I, "Customer List select index was out of range");
                }
                return new Customer();
            }
        }

        //function to print review review is review being printed adminview displays ID if true
        public void PrintReview(Review review, bool adminView)
        {
            Console.WriteLine("*********************************************\n");
            if(adminView){
                Console.WriteLine($"Review Id: {review.Id}");
            }else
            {
                string Name = _restaurantBL.GetCustomerById(review.CustomerId).Name;
                Console.WriteLine($"{Name} Says:");
            }
            
            Console.WriteLine($"{decimal.Round(review.Stars,1)}/5 Stars");
            if(review.textReview != ""){
                Console.WriteLine(review.textReview);
            }
            Console.WriteLine("\n*********************************************");
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
                    if(rating >= 0 && rating <= 5 ){
                        valid = true;
                    }else
                    {
                        Console.WriteLine("Enter a Number between 0 and 5!");
                    }
                }else
                {
                    Console.WriteLine("Enter a Number between 0 and 5!");
                }
            }while(!valid);
            Console.WriteLine("Leave a comment!");
            comment = Console.ReadLine();
            
            if(String.IsNullOrWhiteSpace(comment))
            {
                latestReview = new Review(rating, this.CurrentCustomer.Id, restaurant.Id, "" );
            }else
            {
                latestReview = new Review(rating, this.CurrentCustomer.Id, restaurant.Id, comment);
            }
            _restaurantBL.LeaveReview(latestReview);
            Console.WriteLine($"Successfully left a review for {restaurant.Name}");
        }

        //Adds a new restaurant to the database by creating restaurant object and passing it on to BL
        public void AddRestaurant(){
            string name;
            string address;
            int zip;
            bool valid = false;
            Restaurant restaurantToAdd;
            name = ValidInput("Enter Restaurants Name");
            
            address = ValidInput("Enter Address(NOT INCLUDING ZIP):");
                    do{
                        Console.WriteLine("Enter Restaurant zip: ");
                        if(int.TryParse(Console.ReadLine(), out zip))
                        {
                            valid = true;
                        }else
                        {
                            Console.WriteLine("Not a valid zip!");
                        }
                    }while(!valid);
            List<Restaurant> possibleDuplicate = _restaurantBL.SearchRestaurantsName(name);
            if(possibleDuplicate.Count > 0){
                foreach(Restaurant restaurant in possibleDuplicate)
                {
                    if(restaurant.Name == name && restaurant.Address == address && restaurant.Zip == zip)
                    {
                        Console.WriteLine("Restaurant already registered");
                        return;
                    }
                }
            }
            restaurantToAdd = new Restaurant(name, address, zip);
            restaurantToAdd = _restaurantBL.AddRestaurant(restaurantToAdd);
        }
        
        //returns false if phone number is invalid
        public bool ValidPhoneInput(string phone)
        {
                const string allowed = "1234567890-()";
                
                
                foreach(char c in phone){
                    if(!allowed.Contains(c.ToString())){
                        Console.WriteLine("Invalid character in Phone Number");
                        return false;
                    }
                    
                }
                return true;
        }

        //Adds a new customer to database database by creating restaurant object and passing it on to BL
        public void AddCustomer(){
            string name;
            string pass;
            string pass2;
            string phone;
            string email;
            bool valid = false;
            Customer latestCustomer;
            do
            {
                name = ValidInput("Enter Name: ");
                if(_restaurantBL.GetCustomer(name).Name is null)
                {
                    valid = true;
                }else
                {
                    Console.WriteLine("That Username is taken! please enter a different one");
                }
            }while(!valid);
            valid = false;
            do
            {
                pass = ValidInput("Enter password: ");
                pass2 = ValidInput("Confirm Password: ");
                if(pass == pass2)
                {
                    valid = true;
                }else
                {
                    Console.WriteLine("Entered passwords were different!");
                }
            }while(!valid);
            valid = false;
            do
            {
                phone = ValidInput("Enter phone number: ");
                valid = ValidPhoneInput(phone);
            }while(!valid);
            valid = false;
            do
            {
                email = ValidInput("Enter email: ");
                if(email.Contains("@") && email.Contains(".")){
                    valid = true;
                }else
                {
                    Console.WriteLine("Please enter a valid email address!");
                }
            }while(!valid);
            
            
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
            
            this.CurrentCustomer = _restaurantBL.GetCustomer(name);

            if(this.CurrentCustomer.Name is null){
                Console.WriteLine($"{name} is not a username");
            }else
            {
                pass = ValidInput("Enter Password:: ");
                if(pass == this.CurrentCustomer.Pass){
                    return false;
                }
                Console.WriteLine("Incorrect Password");
            }
            return true;
        }

        //Menu for once you find restaurant, param restaurant is restaurant being viewed
        public void restaurantMenu(Restaurant restaurant){
            bool repeat = true;
            List<Review> reviews;
            reviews = _restaurantBL.FindRatingsByRestaurantId(restaurant);
            Console.WriteLine("\n*********************************************");
            Console.WriteLine($"{restaurant.Name}\n{restaurant.Address} {restaurant.Zip}");
            decimal avg = decimal.Round(_restaurantBL.AverageRating(restaurant),2);
            if ( avg == -1 )
            {
                Console.WriteLine("No ratings for this restaurant yet");
            }else
            {
                Console.WriteLine($"Average rating: {avg}/5 stars");
                Console.WriteLine("Recent Reviews: ");
                try
                {
                    PrintReview(reviews[0], false);
                }
                catch(IndexOutOfRangeException I)
                {
                    Log.Fatal(I, "Index was out of bounds when printing recent review");
                }
            }
            
            
            do{
                reviews = _restaurantBL.FindRatingsByRestaurantId(restaurant);
                Console.WriteLine("\n*********************************************");
                Console.WriteLine("[1] Leave Review for this restaurant");
                Console.WriteLine("[2] See all reviews for this restaurant");
                Console.WriteLine("[3] Find another restaurant (go to previous menu)");
                switch(Console.ReadLine()){
                    case "1":
                        LeaveReview(restaurant);
                    break;

                    case "2":
                        if(reviews.Count == 0){
                            Console.WriteLine("No Reviews");
                        }else{
                            foreach(Review review in reviews){
                                PrintReview(review, false);
                            }
                        }
                    break;

                    case "3":
                        repeat = false;
                    break;

                    default:
                            Console.WriteLine("\n***Not a valid command***");
                            Console.WriteLine("*********************************************");
                    break;

                }

            }while(repeat);
        }


        //menu for viewing users as admin parameter searchedCustomer is customer being viewed
        public void AdminUserMenu(Customer searchedCustomer)
        {
            if(searchedCustomer.Name is null)
            {
                Console.WriteLine($"\n***Not a username***");
            }else
            {
                Console.WriteLine($"{searchedCustomer.Name}: \nID: {searchedCustomer.Id}\nPhone Number: {searchedCustomer.Phone} \n Email: {searchedCustomer.Email}");
                List<Review> searchedCustomerReviews =_restaurantBL.FindReviewsByCustomer(searchedCustomer);
                Console.WriteLine("Most Recent Review:");
                if(searchedCustomerReviews.Count > 0){
                    PrintReview(searchedCustomerReviews[0], true);
                }else
                {
                    Console.WriteLine("User has not left any reviews");
                }
                
                bool repeat = true;
                while(repeat){
                    Console.WriteLine("[0] Exit");
                    Console.WriteLine("[1] See all reviews by this user");
                    Console.WriteLine("[2] Delete this user");
                    switch(Console.ReadLine()){
                    case "0":
                        repeat = false;
                    break;

                    case "1":
                        foreach (Review review in searchedCustomerReviews){
                                PrintReview(review, true);
                        }
                    break;
                    case "2":
                        if(searchedCustomer.Id == this.CurrentCustomer.Id)
                        {
                            Console.WriteLine("You cannot delete this account from itself");
                        }else
                        {
                            Console.WriteLine("Enter Password to continue");
                            if (Console.ReadLine() == CurrentCustomer.Pass){
                                int id;
                                Console.WriteLine("Are you sure you want to delete this account and all it's reviews? Enter the ID of the account to be deleted to confirm");
                                if(int.TryParse(Console.ReadLine(), out id))
                                {
                                    if(id == searchedCustomer.Id){
                                        _restaurantBL.DeleteUser(searchedCustomer);
                                        repeat = false;
                                        Console.WriteLine("User Deleted");
                                    }else
                                    {
                                        Console.WriteLine("Id does not match, aborting delete...");
                                    }   
                                    
                                }else
                                {
                                    Console.WriteLine("Id does not match, aborting delete...");
                                }
                                    
                            }
                        }
                    break;

                    default:
                        Console.WriteLine("\n***Not a valid command***");
                        Console.WriteLine("*********************************************");
                    break;
                    }
                }
            }
        }

        //View a restaurant as an admin Parameter restaurant is restaurant to be viewed
        public void AdminRestaurantMenu(Restaurant restaurant)
        {
            Console.WriteLine("\n*********************************************");
            Console.WriteLine($"ID:{restaurant.Id}\n{restaurant.Name}\n{restaurant.Address} {restaurant.Zip}");
            Console.WriteLine("Enter Password to continue");
            if (Console.ReadLine() == CurrentCustomer.Pass){
                int id;
                Console.WriteLine("Are you sure you want to delete this restaurant and all it's reviews? Enter the ID of the restaurant to be deleted to confirm");
                if(int.TryParse(Console.ReadLine(), out id))
                {
                    if(id == restaurant.Id){
                        _restaurantBL.DeleteRestaurant(restaurant);
                        Console.WriteLine("Restaurant Deleted");
                    }else
                    {
                        Console.WriteLine("Id does not match, aborting delete...");
                    }   
                    
                }else
                {
                    Console.WriteLine("Id does not match, aborting delete...");
                }
                    
            }
        }
    }
}