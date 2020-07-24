using JobApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JobApp.Controllers
{

   [Authorize]
    public class JobsController : Controller
        {
            private ApplicationDbContext db = new ApplicationDbContext();

            // GET: Jobs
            public ActionResult Index()
            {
                var jobs = db.Jobs;
                return View(jobs.ToList());
            }
        [AllowAnonymous]
        public ActionResult GetAllJobs()
        {
            var jobs = db.Jobs;
            return View(jobs.ToList());
        }


        // GET: Jobs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Job job = db.Jobs.Find(id);
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return PartialView(job);
        }

        // GET: Jobs/Create
        public ActionResult Create()
            {
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName");
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(Job job, HttpPostedFileBase upload)
            {
                if (ModelState.IsValid)
                {
                    string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                    upload.SaveAs(path);
                    job.JobImage = upload.FileName;
                    job.UserID = User.Identity.GetUserId();
                    db.Jobs.Add(job);
                    db.SaveChanges();


                var UserID = User.Identity.GetUserId();

                var x = db.Jobs.Where(s => s.UserID == UserID).Include(a=> a.User).ToList();
                //ViewBag.y = db.Users.Where(a => /*a.UserType == "Developer"&*/a.UserName==User.Identity.GetUserId()).Select(s=> s.UserName);
                

                return RedirectToAction("GetJobsByPublisherr2", "home", x);
            }

                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", job.CategoryId);
                return View(job);
            }

            // GET: Jobs/Edit/5
            public ActionResult Edit(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Job job = db.Jobs.Find(id);
                if (job == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", job.CategoryId);
                return View(job);
            }

            // POST: Jobs/Edit/5
            // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
            // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit(Job job, HttpPostedFileBase upload)
            {
                if (ModelState.IsValid)
                {
                    string oldPath = Path.Combine(Server.MapPath("~/Uploads"), job.JobImage);

                    if (upload != null)
                    {
                        System.IO.File.Delete(oldPath);
                        string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                        upload.SaveAs(path);
                        job.JobImage = upload.FileName;
                    }


                    db.Entry(job).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", job.CategoryId);
                return View(job);
            }

            // GET: Jobs/Delete/5
          

        public ActionResult Delete(int id)
        {
            db.Jobs.Remove(db.Jobs.Single(m => m.Id == id));
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
