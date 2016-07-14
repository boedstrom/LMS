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

        // ***
        public ActionResult BackToCourse(int? id)
        {
            return RedirectToAction("Index", "Courses");
        }

        // ***
        public ActionResult ShowActivities(int? id)
        {
            return RedirectToAction("Index", "Activities", new { id });
        }

        // ***
        public ActionResult ShowDocuments(int? id)
        {
            return RedirectToAction("FromModule", "Documents", new { id });
        }

        // GET: Modules
        public ActionResult Index(int? id)
        {
            ShowModulesViewModel courseModules = new ShowModulesViewModel();

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
                //--------------------------------------------------------------------------
                //              int compare = thisDate.CompareTo(thatDate);
                //              if (compare < 0)    thisDate has passed
                //              else if (compare == 0)  Same = today
                //              else // (compareValue > 0)  thisDate is in the future

                //  Check activity start time against module end date
                int compare = thisCourse.EndDate.CompareTo(module.StartDate);

                //  Module has ended when the activity starts
                if (compare < 0)
                {
                    ViewBag.Message = "Activity starts after the module end date";
                    return View(module);
                }
                else
                {
                    //  Check activity end time against module end date
                    compare = thisCourse.EndDate.CompareTo(module.EndDate);

                    //  Module has already ended when the activity ends
                    if (compare < 0)
                    {
                        ViewBag.Message = "Activity ends after the module end date";
                        return View(module);
                    }
                }
                //--------------------------------------------------------------------------

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

        // ***
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
