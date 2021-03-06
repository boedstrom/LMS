﻿using System;
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

        // ***
        public ActionResult ShowDocuments(int? id)
        {
            return RedirectToAction("FromActivity", "Documents", new { id });
        }

        // ***
        public ActionResult BackToModule(int? id)
        {
            Module thisModule = db.Modules.Where(c => c.Id == id).FirstOrDefault();
            return RedirectToAction("Index", "Modules", new { id = thisModule.Course.Id });
        }

        // ***
        public ActionResult BackToStudent(int? id)
        {
            Module thisModule = db.Modules.Where(c => c.Id == id).FirstOrDefault();
            return RedirectToAction("StudentIndex", "Courses", new { id = thisModule.Course.Id });
        }

        public ActionResult BackToCourse(int? id)
        {
            Module thisModule = db.Modules.Where(c => c.Id == id).FirstOrDefault();
            return RedirectToAction("CourseIndex", "Courses", new { id = thisModule.Course.Id });
        }

        // GET: Activities
        public ActionResult Index(int? id)
        {
            ShowActivitiesViewModel moduleActivities = new ShowActivitiesViewModel();

            Module module = db.Modules.Where(c => c.Id == id).FirstOrDefault();
            moduleActivities.ModuleId = module.Id;
            moduleActivities.ModuleName = module.Name;
            moduleActivities.ModuleStart = module.StartDate.Date;
            moduleActivities.ModuleEnd = module.EndDate.Date;
            moduleActivities.Activities = db.Activities.Where(m => m.Module.Id == module.Id).ToList().OrderBy(m => m.StartTime);

            return View(moduleActivities);
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
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(int? id)
        {
            Activity activity = new Activity();
            activity.Module = db.Modules.Where(c => c.Id == id).FirstOrDefault();
            activity.StartTime = activity.Module.StartDate;
            activity.EndTime = activity.Module.StartDate.AddHours(4);
            return View(activity);
        }

        // POST: Activities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Type,StartTime,EndTime,Deadline,Module")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                Module thisModule = db.Modules.Where(c => c.Id == activity.Module.Id).FirstOrDefault();
                //--------------------------------------------------------------------------
                //              int compare = thisDate.CompareTo(thatDate);
                //              if (compare < 0)    thisDate has passed                 -1
                //              else if (compare == 0)  Same = today                    0
                //              else // (compareValue > 0)  thisDate is in the future   1

                //  Check Activity start time against module start date
                int compare = thisModule.StartDate.Date.CompareTo(activity.StartTime.Date);

                //  Activity starts before the module
                if (compare > 0)
                {
                    ViewBag.Message = "Activity starts before the module start date";
                    return View(activity);
                }
                else
                {
                    //  Check activity start time against module end date
                    compare = thisModule.EndDate.Date.CompareTo(activity.StartTime.Date);

                    //  Activity starts before the module end
                    if (compare > 0)
                    {
                        //  Check activity end date against module end date
                        compare = thisModule.EndDate.Date.CompareTo(activity.EndTime.Date);

                        //  Module has ended when the activity ends
                        if (compare < 0)
                        {
                            ViewBag.Message = "Activity ends after the module end date";
                            return View(activity);
                        }
                    }
                    //  Module has ended when the activity starts
                    else
                    {
                        ViewBag.Message = "Activity starts after the module end date";
                        return View(activity);
                    }
                }
                //--------------------------------------------------------------------------
                activity.Module = thisModule;
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Index", "Activities", new { id = activity.Module.Id });
            }

            return View(activity);
        }

        // GET: Activities/Edit/5
        [Authorize(Roles = "Teacher")]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Type,StartTime,EndTime,Deadline,Module")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Activities", new {id = activity.Module.Id });
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        [Authorize(Roles = "Teacher")]
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
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteConfirmed(int id)
        {
            Activity activity = db.Activities.Find(id);
            int moduleid = activity.Module.Id;
            db.Activities.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Index", "Activities", new { id = moduleid });
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
