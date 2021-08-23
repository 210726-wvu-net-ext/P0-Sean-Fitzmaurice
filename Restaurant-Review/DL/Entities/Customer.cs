using System;
using System.Collections.Generic;

#nullable disable

namespace DL.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? IsAdmin { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
