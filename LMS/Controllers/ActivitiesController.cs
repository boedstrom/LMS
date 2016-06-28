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
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult BackToModule(int? id)
        {
            Module thisModule = db.Modules.Where(c => c.Id == id).FirstOrDefault();
            return RedirectToAction("AddModule", "Modules", new { id = thisModule.Course.Id });
        }

        public ActionResult AddActivity(int? id)
        {
            AddActivityViewModel moduleActivities = new AddActivityViewModel();

            Module thisModule = db.Modules.Where(c => c.Id == id).FirstOrDefault();
            moduleActivities.ModuleId = thisModule.Id;
            moduleActivities.ModuleName = thisModule.Name;
            moduleActivities.Activities = db.Activities.Where(m => m.Module.Id == thisModule.Id).ToList();
            return View(moduleActivities);
        }

        // GET: Activities
        public ActionResult Index()
        {
            return View(db.Activities.ToList());
        }

        // GET: Activities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create(int? id)
        {
            Activity activity = new Activity();
            activity.Module = db.Modules.Where(c => c.Id == id).FirstOrDefault();
            return View(activity);
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Type,StartTime,EndTime,Deadline,Module")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                Module thisModule = db.Modules.Where(c => c.Id == activity.Module.Id).FirstOrDefault();
                activity.Module = thisModule;
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("AddActivity","Activities",new {id = activity.Module.Id });
            }

            return View(activity);
        }

        // GET: Activities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Type,StartTime,EndTime,Deadline,Module")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AddActivity", "Activities", new {id = activity.Module.Id });
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            int moduleid = activity.Module.Id;
            db.Activities.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("AddActivity","Activities",new { id = moduleid });
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
