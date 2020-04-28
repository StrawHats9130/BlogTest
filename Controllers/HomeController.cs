using BlogTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlogTest.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            //Get all the BlogPost that are Published
            // return View(db.BlogPosts.Where(foo => foo.Published).OrderByDescending(b => b.Created).ToList());

            //temp string to test with
            return View(db.BlogPosts.OrderByDescending(b => b.Created).ToList());
            //return View(db.BlogPosts.ToList());
            //return View(db.BlogPosts.Where(b=>b.Published).ToList())
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}