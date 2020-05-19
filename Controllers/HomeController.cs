﻿using BlogTest.App_Start;
using BlogTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using BlogTest.Helpers;

namespace BlogTest.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private SearchHelpers searchHelpers = new SearchHelpers();

        public ActionResult Index(int? page, string searchStr)
        {

            ViewBag.Search = searchStr;
            var blogList = searchHelpers.IndexSearch(searchStr);

            int pageSize = 4;
            int pageNumber = (page ?? 1);
           
            return View(blogList.Where(b => b.Published).OrderByDescending(p => p.Created).ToPagedList(pageNumber, pageSize));
            //Get all the BlogPost that are Published
           // return View(db.BlogPosts.Where(foo => foo.Published).OrderByDescending(b => b.Created).ToList());

            //temp string to test with
            //return View(db.BlogPosts.OrderByDescending(b => b.Created).ToList());
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
            //ViewBag.Message = "Your contact page.";
            EmailModel model = new EmailModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: <bold>{0}</bold> {1} </p><p>Message:</p><p>{2}</p>";
                    var from = $"Bens Blog<{WebConfigurationManager.AppSettings["emailFrom"]}>";
                    model.Body = "This is a message from your blog site.  The name and the email of the contacting person is above.";

                    var email = new MailMessage(from, WebConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = "Portfolio Contact Email",
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                    return RedirectToAction("Index", "Home");

                    //return View(new EmailModel());
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }


    }
}