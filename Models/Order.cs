using System;
using System.Collections.Generic;

namespace ecommerce.Models
{
    public class Order: BaseEntity
    {
        public int OrderId {get;set;}
        public int Quantity {get;set;}
        public int UserId {get;set;}
        public User User {get;set;}
        public int ProductId {get;set;}
        public Product Product {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}

    }
}
