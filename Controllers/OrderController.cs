using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ecommerce.Models;

namespace ecommerce.Controllers
{
    public class OrderController: Controller
    {
        private OrderContext _context;

        public OrderController(OrderContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("orders")]
        public IActionResult AllOrders()
        {
            int? loggedUserInt = HttpContext.Session.GetInt32("UserId");
            if (loggedUserInt == null)  
            {
                return RedirectToAction("Register", "User");
            }
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            List<Order> Orders = _context.Orders.Include(order => order.User).Include(order => order.Product).ToList();

            List<User> Users = _context.Users.ToList();
            List<Product> Products = _context.Products.ToList();

            ViewBag.Users = Users;
            ViewBag.Products = Products;
            ViewBag.Orders = Orders;
            // System.Console.WriteLine(Orders[0].User.FirstName);
            return View();
        }
        [HttpPost]
        [Route("add_order")]
        public IActionResult AddOrder(Order order)
        {
            System.Console.WriteLine(order);
            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            _context.Add(order);
            _context.SaveChanges();
            Product myProduct = _context.Products.SingleOrDefault(product => product.ProductId == order.ProductId);
            myProduct.Inventory -= order.Quantity;
            _context.SaveChanges();
            return RedirectToAction("AllOrders");
            
        }

        [HttpGet]
        [Route("products")]
        public IActionResult AllProducts()
        {
            int? loggedUserInt = HttpContext.Session.GetInt32("UserId");
            if (loggedUserInt == null)  
            {
                return RedirectToAction("Register", "User");
            }
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            List<Product> Products = _context.Products.ToList();
            ViewBag.Products = Products;
            return View();
        }
        [HttpPost]
        [Route("add_product")]
        public IActionResult AddProduct(Product product)
        {
            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = DateTime.Now;
            _context.Add(product);
            _context.SaveChanges();
            return RedirectToAction("AllProducts");

        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            List<Product> Products = _context.Products.ToList();
            var newProducts = Products.Take(5);
            

            List<Order> Orders = _context.Orders.OrderByDescending(time => time.CreatedAt).Include(order => order.User).Include(order => order.Product).ToList();
            var newOrders = Orders.Take(3);

            List<User> Customers = _context.Users.OrderByDescending(time => time.CreatedAt).ToList();
            var newCustomers = Customers.Take(3);
            ViewBag.Products = newProducts;
            ViewBag.Orders = newOrders;
            ViewBag.Customers = newCustomers;
            return View();
        }
    }
}
