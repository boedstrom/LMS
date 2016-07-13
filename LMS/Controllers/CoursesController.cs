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
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ***
        public ActionResult CreateActivity(int? id)
        {
            return RedirectToAction("Create", "Activities", new { id });
        }

            // Course Index
        public ActionResult CourseIndex(int? id)
        {
            Course course = db.Courses.Find(id);
            ShowModulesViewModel courseModules = new ShowModulesViewModel();
            courseModules.CourseId = course.Id;
            courseModules.CourseName = course.Name;
            courseModules.CourseDescription = course.Description;
            courseModules.CourseStart = course.StartDate.Date;
            courseModules.CourseEnd = course.EndDate.Date;
            courseModules.Modules = db.Modules.Where(m => m.Course.Id == course.Id).ToList();
            if (courseModules.Modules.Count() == 0)
            {
                courseModules.CourseDescription = "There are no modules for this course";
                return RedirectToAction("StudentNoDataIndex", courseModules);
            }
            return View(courseModules);
        }

        // ***
        public ActionResult ActivityPartial(int? id)
        {
            Course course = db.Courses.Find(id);
            ShowModulesViewModel courseModules = new ShowModulesViewModel();
            courseModules.CourseId = course.Id;
            courseModules.CourseName = course.Name;
            courseModules.CourseDescription = course.Description;
            courseModules.CourseStart = course.StartDate.Date;
            courseModules.CourseEnd = course.EndDate.Date;
            courseModules.Modules = db.Modules.Where(m => m.Course.Id == course.Id).ToList();
            Module currentModule = courseModules.Modules.FirstOrDefault();

            int compareEnd = course.EndDate.CompareTo(DateTime.Today);

            //  Course has not ended yet, check start date
            if (compareEnd > 0)
            {
                int compareStart = course.StartDate.CompareTo(DateTime.Today);

                // Course has not started, show first activities for first module
                if (compareStart > 0)
                {
                    currentModule = courseModules.Modules.FirstOrDefault();
                }
                // Course has already started or starts today, show activities for current module            
                else
                {
                    foreach (var module in courseModules.Modules)
                    {
                        compareStart = module.StartDate.CompareTo(DateTime.Today);

                        //  Module has started, check end date
                        if (compareStart < 0)
                        {
                            compareEnd = module.EndDate.CompareTo(DateTime.Today);

                            // Module has not ended yet, show activities for this module
                            if (compareEnd > 0)
                            {
                                currentModule = module;
                                break;
                            }
                            // Module ends today, show activities for this module
                            else if (compareEnd == 0)
                            {
                                currentModule = module;
                                break;
                            }
                        }
                        // Module starts today, show activities for this module
                        else if (compareStart == 0)
                        {
                            currentModule = module;
                            break;
                        }
                    }
                }
            }
            // Course has already ended or ends today, show activities for last module            
            else
            {
                currentModule = courseModules.Modules.LastOrDefault();
            }

            ShowActivitiesViewModel studentActivityView = new ShowActivitiesViewModel();
            studentActivityView.ModuleId = currentModule.Id;
            studentActivityView.ModuleName = currentModule.Name;
            studentActivityView.ModuleStart = currentModule.StartDate.Date;
            studentActivityView.ModuleEnd = currentModule.EndDate.Date;
            studentActivityView.Activities = db.Activities.Where(m => m.Module.Id == currentModule.Id).ToList().OrderBy(m => m.StartTime);
            if (studentActivityView.Activities.Count() == 0)
            {
                studentActivityView.ModuleName = "There are no activities for the current module";
                return PartialView("StudentNoDataPartial", studentActivityView);
            }
            return PartialView("ActivityPartial", studentActivityView);
        }

        // ***
        public ActionResult StudentNoDataPartial(ShowActivitiesViewModel studentActivityView)
        {
            return PartialView("StudentNoDataPartial", studentActivityView);
        }

        // Student Index
        public ActionResult StudentIndex()
        {
            ApplicationUser student = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            Course course = student.Course;
            ShowModulesViewModel courseModules = new ShowModulesViewModel();
            courseModules.CourseId = course.Id;
            courseModules.CourseName = course.Name;
            courseModules.CourseDescription = course.Description;
            courseModules.CourseStart = course.StartDate.Date;
            courseModules.CourseEnd = course.EndDate.Date;
            courseModules.Modules = db.Modules.Where(m => m.Course.Id == course.Id).ToList();
            if (courseModules.Modules.Count() == 0)
            {
                courseModules.CourseDescription = "There are no modules for this course";
                return RedirectToAction("StudentNoDataIndex", courseModules);
            }
            return View(courseModules);
        }

        // Some data missing, show "error" view
        public ActionResult StudentNoDataIndex(ShowModulesViewModel courseModules)
        {
            return View(courseModules);
        }

        // Student Modules
        public ActionResult ViewActivities(int? id)
        {
            return RedirectToAction("Index", "Activities", new { id });
        }

        // GET: Courses
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        // ***
        public ActionResult ShowModules(int? id)
        {
            return RedirectToAction("Index", "Modules", new { id });
        }

        // ***
        public ActionResult ShowActivitiesForPartialModules(int? id)
        {
            return RedirectToAction("ShowActivities", "Modules", new { id });
        }

        // ***
        public ActionResult ShowUsers(int? id)
        {
            return RedirectToAction("Index", "Users", new { id });
        }

        // ***
        public ActionResult ShowCourseDocuments(int? id)
        {
            return RedirectToAction("FromCourse", "Documents", new { id });
        }

        // ***
        public ActionResult ShowModuleDocuments(int? id)
        {
            return RedirectToAction("FromModule", "Documents", new { id });
        }

        // ***
        public ActionResult ShowActivityDocuments(int? id)
        {
            return RedirectToAction("FromActivity", "Documents", new { id });
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            Course course = new Course();
            course.StartDate = DateTime.Now;
            course.EndDate = DateTime.Now.AddDays(1);
            return View(course);
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
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
