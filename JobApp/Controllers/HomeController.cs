using JobApp.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace JobApp.Controllers
{
    public class HomeController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            //ViewBag.catID = db.Categories.First()
            return View(db.Categories.Include("Jobs").ToList());
        }

        public ActionResult GetJobByCategory(int id)
        {
            var d = db.Jobs.Where(x => x.CategoryId == id);
            return View(d);
        }


        public ActionResult Details(int JobId)
        {
            var job = db.Jobs.Find(JobId);

            if (job == null)
            {
                return HttpNotFound();
            }
            Session["JobId"] = JobId;
            return View(job);
        }


        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Apply(string Message, HttpPostedFileBase PathFile)
        {



            var UserId = User.Identity.GetUserId();
            var JobId = (int)Session["JobId"];

            var check = db.ApplyForJobs.Where(a => a.JobId == JobId && a.UserId == UserId).ToList();

            if (check.Count < 1)
            {
                string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(PathFile.FileName));
                PathFile.SaveAs(path);

                var job = new ApplyForJob();

                job.UserId = UserId;
                job.JobId = JobId;
                job.Message = Message;
                job.ApplyDate = DateTime.Now;
                job.PathFile = "~/Images/" + PathFile.FileName;



                db.ApplyForJobs.Add(job);
                db.SaveChanges();
                ViewBag.Result = "Added Successfuly !";
            }
            else
            {
                ViewBag.Result = "sorry , you have applied to this job before!";
            }





            return View();

        }


        [Authorize]
        public ActionResult GetJobsByUser()
        {
            var UserId = User.Identity.GetUserId();
            var Jobs = db.ApplyForJobs.Where(a => a.UserId == UserId);
            return View(Jobs.ToList());
        }

        [Authorize]
        public ActionResult DetailsOfJob(int id)
        {
            var job = db.ApplyForJobs.Find(id);

            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        //[Authorize]
        //public ActionResult GetJobsByPublisherr()
        //{
        //    var UserID = User.Identity.GetUserId();

        //    var x = db.Jobs.Where(s => s.UserID == UserID).Include(a=>a.User).ToList();
        //    

        //    return View(x);
        //}
        [Authorize]
        public ActionResult GetJobsByPublisherr2()
        {
            var UserID = User.Identity.GetUserId();

            var Jobs = from app in db.ApplyForJobs
                       join job in db.Jobs
                       on app.JobId equals job.Id
                       where job.User.Id == UserID
                       select app;

            var grouped = from j in Jobs 
                          group j by j.job.JobTitle
                          into gr
                          select new JobsViewModel
                          {
                              JobTitle = gr.Key,
                              Items = gr
                          };

            return View(grouped.ToList());
        }


        public ActionResult Edit(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        [HttpPost]
        public ActionResult Edit(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                job.ApplyDate = DateTime.Now;
                db.Entry(job).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");
            }
            return View(job);
        }


        public ActionResult Delete(int id)
        {

            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(ApplyForJob job)
        {
            // TODO: Add delete logic here
            var myJob = db.ApplyForJobs.Find(job.Id);
            db.ApplyForJobs.Remove(myJob);
            db.SaveChanges();
            return RedirectToAction("GetJobsByUser");

        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("mohamedabdelmageed99@gmail.com", "mohamedabdelmageed99");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("mohamedabdelmageed@gmail.com"));
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true;
            string body = "Sender Name: " + contact.Name + "<br>" +
                            "E_mail: " + contact.Email + "<br>" +
                            "Title: " + contact.Subject + "<br>" +
                            "Message: <b>" + contact.Message + "</b>";
            mail.Body = body;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(mail);
            return RedirectToAction("Index");
        }


        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string search)
        {
            var result = db.Jobs.Where(a => a.JobTitle.Contains(search)
            || a.JobContent.Contains(search)
            || a.Category.CategoryName.Contains(search)
            || a.Category.CategoryDescription.Contains(search)).ToList();

            return View(result);
        }
    }
}