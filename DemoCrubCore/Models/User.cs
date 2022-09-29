﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DemoCrubCore.Models
{
    public partial class User
    {
        public User()
        {
            SaleOrders = new HashSet<SaleOrder>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }

        public virtual ICollection<SaleOrder> SaleOrders { get; set; }
    }
}
