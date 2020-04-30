using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlogTest.Helpers;
using BlogTest.Models;
using PagedList;
using PagedList.Mvc;

namespace BlogTest.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private SearchHelpers searchHelpers = new SearchHelpers();
        // GET: BlogPosts
        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr;
            var blogList = searchHelpers.IndexSearch(searchStr);

            int pageSize = 4;
            int pageNumber =( page ?? 1);
           
            return View(blogList.OrderByDescending(p => p.Created).ToPagedList(pageNumber,pageSize));
        }

        // GET: BlogPosts/Details/5
        [AllowAnonymous]
        public ActionResult Details(string slug)
        {
            if (slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.FirstOrDefault(p => p.Slug == slug);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Abstract,Body,MediaURL,Published,CommentBody")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                //How to instanciate objects
                //var helpers = new StringUtilities();
                //var slug = helpers.URLFriendly(blogPost.Title);

                var slug = StringUtilities.URLFriendly(blogPost.Title);

                if (string.IsNullOrWhiteSpace(slug))
                {
                    ModelState.AddModelError("Title", "The Title can not be empty.");
                    return View(blogPost);
                }
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    //isolating file name
                    var justFileName = Path.GetFileNameWithoutExtension(image.FileName);
                    //running through slug checker
                    justFileName = StringUtilities.URLFriendly(justFileName);
                    //adding time stamp width ticks
                    justFileName = $"{justFileName}-{DateTime.Now.Ticks}";
                    //adding the extension back on
                    justFileName = $"{justFileName}{Path.GetExtension(image.FileName)}";

                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), justFileName));
                    blogPost.MediaURL = "/Uploads/" + justFileName;

                }
                if (db.BlogPosts.Any(p => p.Slug == slug))
                {
                    ModelState.AddModelError("", $"The title '{blogPost.Title}'  has been used before");
                    return RedirectToAction("Index");
                }
                
                blogPost.Slug = slug;
                blogPost.Created = DateTime.Now;
                db.BlogPosts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5

        public ActionResult Edit(string slug)
        {
            if (slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.FirstOrDefault(p => p.Slug == slug);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Edit([Bind(Include = "Id,Created,Title,Slug,Abstract,Body,MediaURL,Published")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    //isolating file name
                    var justFileName = Path.GetFileNameWithoutExtension(image.FileName);
                    //running through slug checker
                    justFileName = StringUtilities.URLFriendly(justFileName);
                    //adding time stamp width ticks
                    justFileName = $"{justFileName}-{DateTime.Now.Ticks}";
                    //adding the extension back on
                    justFileName = $"{justFileName}{Path.GetExtension(image.FileName)}";

                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), justFileName));
                    blogPost.MediaURL = "/Uploads/" + justFileName;
                }

                
                blogPost.Updated = DateTime.Now;
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.FirstOrDefault(p => p.Id == id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            BlogPost blogPost = db.BlogPosts.FirstOrDefault(p => p.Id == id);
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
