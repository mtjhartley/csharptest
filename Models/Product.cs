using System;
using System.Collections.Generic;

namespace ecommerce.Models
{
    public class Product: BaseEntity
    {
        public int ProductId {get;set;}
        public string Name {get;set;}
        public int Inventory {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<Order> Orders {get;set;}
        public Product()
        {
            Orders = new List<Order>();
        }
    }
}