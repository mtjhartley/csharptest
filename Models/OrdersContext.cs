using Microsoft.EntityFrameworkCore;

namespace ecommerce.Models
{
    public class OrderContext: DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

        public DbSet<User> users {get;set;}
        public DbSet<Product> products {get;set;}
        public DbSet<Order> orders {get;set;}
    }
}