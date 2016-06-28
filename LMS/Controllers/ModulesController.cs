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

        public ActionResult AddModule(int? id)
        {
            AddModuleViewModel courseModules = new AddModuleViewModel();

            Course thisCourse = db.Courses.Where(c => c.Id == id).FirstOrDefault();
            courseModules.CourseId = thisCourse.Id;
            courseModules.CourseName = thisCourse.Name;
            courseModules.Modules = db.Modules.Where(m => m.Course.Id == thisCourse.Id).ToList();
            return View(courseModules);
        }

        public ActionResult AddActivity(int? id)
        {
            return RedirectToAction("AddActivity", "Activities", new { id = id });
        }

        // GET: Modules
        public ActionResult Index()
        {
            return View(db.Modules.ToList());
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
        public ActionResult Create(int? id)
        {
            Module module = new Module();
            module.Course = db.Courses.Where(c => c.Id == id).FirstOrDefault();
            return View(module);
        }

        // POST: Modules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,Course")] Module module)
        {
            if (ModelState.IsValid)
            {
                Course thisCourse = db.Courses.Where(c => c.Id == module.Course.Id).FirstOrDefault();
                module.Course = thisCourse;
                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToAction("AddModule", "Modules", new { id = module.Course.Id });
            }

            return View(module);
        }

        // GET: Modules/Edit/5
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
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,Course")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("AddModule", "Modules", new { id = module.Course.Id });
            }
            return View(module);
        }

        // GET: Modules/Delete/5
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
        public ActionResult DeleteConfirmed(int id)
        {
            Module module = db.Modules.Find(id);
            int courseId = module.Course.Id;
            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("AddModule", "Modules", new { id = courseId });
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
