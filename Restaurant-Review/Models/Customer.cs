using System;

namespace Models{
    public class Customer{
        public Customer(){}

        public Customer(string name, string pass, string email, string phone, bool? admin){
            this.Pass = pass;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Admin = admin;
        }
        public Customer(int id, string name, string pass, string email, string phone, bool? admin){
            this.Id = id;
            this.Pass = pass;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
            this.Admin = admin;
        }
        public int Id{ get; set; }
        public string Name{ get; set; }
        public string Pass{ get; set; }
        public string Phone{ get; set; }
        public string Email{ get; set; }
        public bool? Admin{ get; set; }
        
    }
}