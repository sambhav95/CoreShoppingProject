using System;
using System.Collections.Generic;
using System.Linq;
using CoreEcommerceUserPanal.Helpers;
using CoreEcommerceUserPanal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreEcommerceUserPanal.Controllers
{
    public class CustomersController : Controller
    {
        ShoppingProjectFinalContext context = new ShoppingProjectFinalContext();
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customers cust)
        {
            context.Customers.Add(cust);
            context.SaveChanges();
            HttpContext.Session.SetString("logout", cust.UserName);
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]

        public ActionResult Login(string username, string password)
        {
           
            
            var user = context.Customers.Where(a => a.UserName == username).SingleOrDefault();
            ViewBag.cust = user;
            if (user == null)
            {
                ViewBag.Error = "Invalid Credentials";
                return View("Login");
            }
            else
            {
                var userName = user.UserName;
                int custId = ViewBag.cust.CustomerId;
                if (username != null && password != null && username.Equals(userName) && password.Equals(user.Password))
                {
                    HttpContext.Session.SetString("uname", username);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cust", user);
                    HttpContext.Session.SetString("logout", userName);
                    return RedirectToAction("Index", "Home", new
                    {
                        @id = custId
                    });
                }
                else
                {
                    ViewBag.Error = "Invalid credentials";
                    return View("Index");
                }
            }
        }
        [Route("Logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("uname");
            HttpContext.Session.Remove("logout");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult custEdit()
        {
            Customers cus1 = SessionHelper.GetObjectFromJson<Customers>(HttpContext.Session, "cust");
            return View(cus1);
        }
        [HttpPost]
        public IActionResult custEdit(int id, Customers customer)
        {
            var c = context.Customers.Where(x => x.UserName == customer.UserName).SingleOrDefault();
            c.FirstName = customer.FirstName;
            c.LastName = customer.LastName;
            c.UserName = customer.UserName;
            c.EmailId = customer.EmailId;
            c.Address = customer.Address;
            c.PhoneNo = customer.PhoneNo;
            c.Country = customer.Country;
            c.State = customer.State;
            c.Zip = customer.Zip;

            context.SaveChanges();
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cust", c);
            return RedirectToAction("Index", "Home", new { @id = customer.UserName });
        }


        public IActionResult Profile(int id, Customers customer)
        {
            Customers cus1 = SessionHelper.GetObjectFromJson<Customers>(HttpContext.Session, "cust");

            return View(cus1);
        }
        public IActionResult password()
        {

            return View();
        }
        [HttpPost]
        public IActionResult password(string oldpassword, string newpassword, string newpassword1)
        {

            Customers c = SessionHelper.GetObjectFromJson<Customers>(HttpContext.Session, "cust");
            if (oldpassword == c.Password && newpassword == newpassword1)
            {
                Customers cus = context.Customers.Where(x => x.UserName == c.UserName).SingleOrDefault();
                cus.Password = newpassword;
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cust", cus);
                context.SaveChanges();
            }
            else
            {
                ViewBag.Error = "Invalid credentials";
                return View("password");
            }



            return RedirectToAction("Login", "Customers");
        }

        public IActionResult OrderHistory()
        {
            Customers c = SessionHelper.GetObjectFromJson<Customers>(HttpContext.Session, "cust");
            List<Orders> ord = context.Orders.Where(x => x.CustomerId == c.CustomerId).ToList();
            ViewBag.ord = ord;
            return View();
        }
        public IActionResult OrderDetail(int id)
        {
            List<OrderProducts> op = new List<OrderProducts>();
            List<Products> products = new List<Products>();
            op = context.OrderProducts.Where(x => x.OrderId == id).ToList();
            foreach (var item in op)
            {
                Products c = context.Products.Where(x => x.ProductId == item.ProductId).SingleOrDefault();
                products.Add(c);
            }
            ViewBag.p = products;
            return View();
        }


    }
}