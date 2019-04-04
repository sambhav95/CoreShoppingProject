using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreEcommerceUserPanal.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreEcommerceUserPanal.Controllers
{
    public class ProductCategoryController : Controller
    {
        ShoppingProjectFinalContext context = new ShoppingProjectFinalContext();
        public IActionResult Index()
        {

            var pc = context.Categories.ToList();
            return View(pc);
        }
        public IActionResult ProductDisplay(int id)
        {
            var p = context.Products.Where(x => x.ProductCategoryId == id).ToList();
            return View(p);
        }
    }
}