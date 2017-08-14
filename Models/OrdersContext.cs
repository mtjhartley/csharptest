using Microsoft.EntityFrameworkCore;

namespace ecommerce.Models
{
    public class OrderContext: DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

        public DbSet<User> Users {get;set;}
        public DbSet<Product> Products {get;set;}
        public DbSet<Order> Orders {get;set;}
    }
}