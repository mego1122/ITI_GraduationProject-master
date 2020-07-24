using JobApp.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobApp.Controllers
{
    [Authorize(Roles = "Admin")]

    public class RolesController : Controller
    {
        private ApplicationDbContext db;

        public RolesController()
        {
            db = new ApplicationDbContext();
        }


        // GET: Roles
        public ActionResult GetAllRoles()
        {
            return View(db.Roles.ToList());
        }

        
        [HttpGet]
        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRole(IdentityRole role)
        {


            try
            {

                if (ModelState.IsValid)
                {
                    db.Roles.Add(role);
                    db.SaveChanges();

                    return RedirectToAction("GetAllRoles");
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return View(role);

        }

        public ActionResult Delete(string id)
        {
            db.Roles.Remove(db.Roles.Single(m => m.Id == id));
            db.SaveChanges();

            return RedirectToAction("GetAllRoles");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            return View(db.Roles.Single(u => u.Id == id));
        }
        [HttpPost]
        public ActionResult Edit(IdentityRole role)
        {
            IdentityRole oldRole = db.Roles.Single(us => us.Id == role.Id);
            if (ModelState.IsValid)
            {
                oldRole.Name = role.Name;
            }
            db.SaveChanges();

            return RedirectToAction("GetAllRoles");

        }


        public ActionResult Details(string id)
        {
            return View(db.Roles.Find(id));
        }
    }
}