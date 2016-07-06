using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;

namespace LMS.Controllers
{
    [Authorize]
    public class ModulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult BackToCourse(int? id)
        {
            return RedirectToAction("Index", "Courses");
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult AddActivity(int? id)
        {
            return RedirectToAction("Index", "Activities", new { id });
        }

        // 
        [Authorize(Roles = "Teacher")]
        public ActionResult AddDocuments(int? id)
        {
            return RedirectToAction("FromModule", "Documents", new { id });
        }

        // GET: Modules
        public ActionResult Index(int? id)
        {
            AddModuleViewModel courseModules = new AddModuleViewModel();

            Course course = db.Courses.Where(c => c.Id == id).FirstOrDefault();
            courseModules.CourseId = course.Id;
            courseModules.CourseName = course.Name;
            courseModules.CourseDescription = course.Description;
            courseModules.CourseStart = course.StartDate.Date;
            courseModules.CourseEnd = course.EndDate.Date;
            courseModules.Modules = db.Modules.Where(m => m.Course.Id == course.Id).ToList();
            return View(courseModules);
        }

        // GET: Modules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // GET: Modules/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(int? id)
        {
            Module module = new Module();
            module.StartDate = DateTime.Now;
            module.EndDate = DateTime.Now.AddDays(1);
            module.Course = db.Courses.Where(c => c.Id == id).FirstOrDefault();
            return View(module);
        }

        // POST: Modules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,Course")] Module module)
        {
            if (ModelState.IsValid)
            {
                Course thisCourse = db.Courses.Where(c => c.Id == module.Course.Id).FirstOrDefault();
                module.Course = thisCourse;
                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToAction("Index", "Modules", new { id = module.Course.Id });
            }

            return View(module);
        }

        // GET: Modules/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,Course")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Modules", new { id = module.Course.Id });
            }
            return View(module);
        }

        // GET: Modules/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteConfirmed(int id)
        {
            Module module = db.Modules.Find(id);
            int courseId = module.Course.Id;
            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Index", "Modules", new { id = courseId });
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
