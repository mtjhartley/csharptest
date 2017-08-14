using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ecommerce.Models;

namespace ecommerce.Controllers
{
    public class UserController: Controller
    {
        private OrderContext _context;
        public UserController(OrderContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Register()
        {
            ViewBag.Errors = new List<string>();
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult HandleRegister(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                User NewUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                NewUser.Password = Hasher.HashPassword(NewUser, model.Password);
                
                _context.Add(NewUser);
                _context.SaveChanges();
                //query the database 
                //hash teh password 
                User justEnteredPerson = _context.users.SingleOrDefault(user => user.Email == model.Email);
                HttpContext.Session.SetString("UserName", justEnteredPerson.FirstName);
                HttpContext.Session.SetInt32("UserId", justEnteredPerson.UserId);

                // return RedirectToAction("Success");
                return RedirectToAction("AllOrders", "Order");
                // return RedirectToAction("Account", "Transaction", new { UserId = justEnteredPerson.UserId}); //change this line!
                //add the new user to the database!
            }
            System.Console.WriteLine("Not Valid!");
            ViewBag.Errors = new List<string>();
            return View("Register");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string PasswordToCheck)
        {
            User loggedUser = _context.users.SingleOrDefault(user => user.Email == Email);
            // System.Console.WriteLine(loggedUser);
            // System.Console.WriteLine(loggedUser.Email);
            if (loggedUser != null && PasswordToCheck != null)
            {
                var Hasher = new PasswordHasher<User>();
                if (0 != Hasher.VerifyHashedPassword(loggedUser, loggedUser.Password, PasswordToCheck))
                {
                    HttpContext.Session.SetString("UserName", loggedUser.FirstName);
                    HttpContext.Session.SetInt32("UserId", loggedUser.UserId);
                    return RedirectToAction("AllOrders", "Order"); //change this line!
                    // return RedirectToAction("Success");
                }
            }
            // System.Console.WriteLine(loggedUser.Password);
            // System.Console.WriteLine(PasswordToCheck);
        
            ViewBag.Errors = new List<string>();
            ViewBag.Errors.Add("Invalid Login Credentials.");
            return View("Register");
        }

        [HttpGet]
        [Route("success")]
        public IActionResult Success()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        [HttpGet]
        [Route("customers")]
        public IActionResult AllUsers()
        {
            int? loggedUserInt = HttpContext.Session.GetInt32("UserId");
            if (loggedUserInt == null)  
            {
                return RedirectToAction("Register", "User");
            }
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            List<User> Users = _context.users.ToList();
            ViewBag.Users = Users;
            return View();

        }
        [HttpPost]
        [Route("delete_customer")]
        public IActionResult DeleteUser(int UserId)
        {
            System.Console.WriteLine(UserId);
            User RetrievedUser = _context.users.SingleOrDefault(user => user.UserId == UserId);
            _context.users.Remove(RetrievedUser);
            _context.SaveChanges();

            return RedirectToAction("AllUsers");
        }

    }
}
