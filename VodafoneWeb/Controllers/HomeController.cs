using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
﻿using Microsoft.AspNet.Identity;
﻿using VodafoneWeb.Models;

namespace VodafoneWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewData["users"] = db.Users.Select(u => new SalesUserViewModule { UserId = u.Id, Username = u.UserName });
            ViewData["products"] = db.Products.Select(p => new ProductViewModel() { ProductId = p.ID, ProductName = p.Name });
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
