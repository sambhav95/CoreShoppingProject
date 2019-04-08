using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreEcommerceUserPanal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreEcommerceUserPanal.Controllers
{
    public class BrandController : Controller
    {
        ShoppingProjectFinalContext context = new ShoppingProjectFinalContext();
        public ViewResult Index()
        {
            var brand = context.Brands.ToList();
            return View(brand);
        }
        public IActionResult ProductDisplay(int id)
        {
            var p = context.Products.Where(x => x.BrandId == id).ToList();
            return View(p);
        }
    }
}