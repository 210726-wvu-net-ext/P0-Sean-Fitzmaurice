using System;

namespace Models{
    public class Restaurant {
        public Restaurant(){}
        public Restaurant(string name, string address, int zip){
            this.Zip = zip;
            this.Address = address;
            this.Name = name;
        }
        public Restaurant(int id, string name, string address, int zip){
            this.Id = id;
            this.Zip = zip;
            this.Address = address;
            this.Name = name;
        }
        public int Id {get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Zip { get; set; }
    }
        
}
