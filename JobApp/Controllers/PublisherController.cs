using JobApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobApp.Controllers
{
    public class PublisherController : Controller
    {
        private ApplicationDbContext db;

        public PublisherController()
        {
            db = new ApplicationDbContext();

        }

     
        public ActionResult ShowProfile()
        {
            string id = User.Identity.GetUserId();
            var Publisher = db.Users.Single(a => a.Id == id);
            return View(Publisher);

        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            string Id = User.Identity.GetUserId();
            var Publisher = db.Users.Single(a => a.Id == Id);
            return PartialView(Publisher);

        }


        [HttpPost]
        public ActionResult EditProfile(ApplicationUser _publisher)
        {
            string Id = User.Identity.GetUserId();
            var OldPublisher = db.Users.Single(a => a.Id == Id);
            if (ModelState.IsValid)
            {
                OldPublisher.UserName = _publisher.UserName;
                OldPublisher.Email = _publisher.Email;
            }
            db.SaveChanges();

            return RedirectToAction("ShowProfile");

        }

    }
}