using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace LMS.Controllers
{
    public class UserViewModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult BackToCourse(int? id)
        {
            return RedirectToAction("Index", "Courses");
        }

        public ActionResult AddUser(int? id)
        {
            AddUserViewModel courseUsers = new AddUserViewModel();

            Course thisCourse = db.Courses.Where(c => c.Id == id).FirstOrDefault();
            courseUsers.CourseId = thisCourse.Id;
            courseUsers.CourseName = thisCourse.Name;
            courseUsers.Users = db.Users.Where(u => u.Course.Id == thisCourse.Id).ToList();
            return View(courseUsers);
        }


        // GET: UserViewModels
        public ActionResult Index()
        {
            return View(db.UserViewModels.ToList());
        }

        // GET: UserViewModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserViewModel userViewModel = db.UserViewModels.Find(id);
            if (userViewModel == null)
            {
                return HttpNotFound();
            }
            return View(userViewModel);
        }

        // GET: UserViewModels/Create
        public ActionResult Create(int? id)
        {
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Course = db.Courses.Where(c => c.Id == id).FirstOrDefault();
            return View(userViewModel);
        }

        // POST: UserViewModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,DefaultPassword,UserType,Course")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var uStore = new UserStore<ApplicationUser>(db);
                var uManager = new UserManager<ApplicationUser>(uStore);

                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email };
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;

                Course thisCourse = db.Courses.Where(c => c.Id == userViewModel.Course.Id).FirstOrDefault();
                user.Course = thisCourse;

                uManager.Create(user, userViewModel.DefaultPassword);
                
                db.SaveChanges();
                return RedirectToAction("AddUser", "UserViewModels", new { id = userViewModel.Course.Id });
            }

            return View(userViewModel);
        }

        // GET: UserViewModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserViewModel userViewModel = db.UserViewModels.Find(id);
            if (userViewModel == null)
            {
                return HttpNotFound();
            }
            return View(userViewModel);
        }

        // POST: UserViewModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,DefaultPassword,UserType,Course")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var uStore = new UserStore<ApplicationUser>(db);
                var uManager = new UserManager<ApplicationUser>(uStore);

                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email };
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;

                Course thisCourse = db.Courses.Where(c => c.Id == userViewModel.Course.Id).FirstOrDefault();
                user.Course = thisCourse;

                uManager.Update(user);
                db.Entry(userViewModel).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("AddUser", "UserViewModels", new { id = userViewModel.Course.Id });
            }
            return View(userViewModel);
        }

        // GET: UserViewModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserViewModel userViewModel = db.UserViewModels.Find(id);
            if (userViewModel == null)
            {
                return HttpNotFound();
            }
            return View(userViewModel);
        }

        // POST: UserViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Module module = db.Modules.Find(id);
            //int courseId = module.Course.Id;
            //db.Modules.Remove(module);
            UserViewModel userViewModel = db.UserViewModels.Find(id);
            db.UserViewModels.Remove(userViewModel);
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
