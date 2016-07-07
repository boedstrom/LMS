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
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult BackToCourse(int? id)
        {
            return RedirectToAction("Index", "Courses");
        }

        public ActionResult BackToStudent(int? id)
        {
            return RedirectToAction("StudentIndex", "Courses", new { id });
        }

        // GET: UserViewModels
        public ActionResult Index(int? id)
        {
            ShowUsersViewModel courseUsers = new ShowUsersViewModel();

            Course course = db.Courses.Where(c => c.Id == id).FirstOrDefault();
            courseUsers.CourseId = course.Id;
            courseUsers.CourseName = course.Name;
            courseUsers.CourseStart = course.StartDate.Date;
            courseUsers.CourseEnd = course.EndDate.Date;

            courseUsers.Users = db.Users.Where(u => u.Course.Id == course.Id).ToList();
            return View(courseUsers);
        }

        public ActionResult TeacherIndex()
        {


            TeacherViewModel teachers = new TeacherViewModel();

            var roleId = db.Roles.FirstOrDefault(x => x.Name == "Teacher").Id;
           teachers.Teachers = db.Users.Where(u => u.Roles.FirstOrDefault().RoleId == roleId).ToList();
            return View(teachers);
        }
        // GET
        public ActionResult CreateTeacher()
        {

           
            return View();

        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTeacher([Bind(Include = "FirstName,LastName,Email,DefaultPassword,UserType")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var uStore = new UserStore<ApplicationUser>(db);
                var uManager = new UserManager<ApplicationUser>(uStore);

                var rStore = new RoleStore<IdentityRole>(db);
                var rManager = new RoleManager<IdentityRole>(rStore);
                var role = rManager.FindByName("Teacher");

                var user = new ApplicationUser {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                };


                var result = uManager.Create(user, userViewModel.DefaultPassword);
                if (result.Succeeded)
                {
                    user = uManager.FindById(user.Id);
                    uManager.AddToRole(user.Id, role.Name);
                } else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return View("CreateTeacher");
                }

                db.SaveChanges();
                return RedirectToAction("TeacherIndex", "UserViewModels");
            }

            return View(userViewModel);
        }

        // GET: UserViewModels/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Id = user.Id;
            userViewModel.FirstName = user.FirstName;
            userViewModel.LastName = user.LastName;
            userViewModel.Email = user.Email;
            userViewModel.Course = user.Course;
            return View(userViewModel);
        }


        public ActionResult DetailsTeacher(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }



            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Id = user.Id;
            userViewModel.FirstName = user.FirstName;
            userViewModel.LastName = user.LastName;
            userViewModel.Email = user.Email;

            return View(userViewModel);
        }

        // GET: UserViewModels/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(int? id)
        {
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Course = db.Courses.Where(c => c.Id == id).FirstOrDefault();
            return View(userViewModel);
        }

        // POST: UserViewModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,DefaultPassword,UserType,Course")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var rStore = new RoleStore<IdentityRole>(db);
                var rManager = new RoleManager<IdentityRole>(rStore);
                var role =  rManager.FindByName("Student");

                var uStore = new UserStore<ApplicationUser>(db);
                var uManager = new UserManager<ApplicationUser>(uStore);

                var user = new ApplicationUser { UserName = userViewModel.Email, Email = userViewModel.Email };
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;

                Course thisCourse = db.Courses.Where(c => c.Id == userViewModel.Course.Id).FirstOrDefault();
                user.Course = thisCourse;

                uManager.Create(user, userViewModel.DefaultPassword);
                uManager.AddToRole(user.Id, role.Name);

                db.SaveChanges();
                return RedirectToAction("Index", "UserViewModels", new { id = userViewModel.Course.Id });
            }

            return View(userViewModel);
        }

        // GET: UserViewModels/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Id = user.Id;
            userViewModel.FirstName = user.FirstName;
            userViewModel.LastName = user.LastName;
            userViewModel.Email = user.Email;
            userViewModel.Course = user.Course;
            return View(userViewModel);
        }

        // POST: UserViewModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,DefaultPassword,UserType,Course")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var uStore = new UserStore<ApplicationUser>(db);
                var uManager = new UserManager<ApplicationUser>(uStore);

                ApplicationUser user = db.Users.Where(u => u.Id == userViewModel.Id).FirstOrDefault();
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.Email = userViewModel.Email;
                user.Course = db.Courses.Where(c => c.Id == userViewModel.Course.Id).FirstOrDefault();

                uManager.Update(user);
                db.SaveChanges();
                return RedirectToAction("Index", "UserViewModels", new { id = userViewModel.Course.Id });
            }
            return View(userViewModel);
        }

        // GET: UserViewModels/EditTeacher/5
        public ActionResult EditTeacher(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Id = user.Id;
            userViewModel.FirstName = user.FirstName;
            userViewModel.LastName = user.LastName;
            userViewModel.Email = user.Email;
            return View(userViewModel);
        }



        //Post
        [HttpPost]
        public ActionResult EditTeacher([Bind(Include = "Id,FirstName,LastName,Email,DefaultPassword,UserType")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var uStore = new UserStore<ApplicationUser>(db);
                var uManager = new UserManager<ApplicationUser>(uStore);

                ApplicationUser user = db.Users.Where(u => u.Id == userViewModel.Id).FirstOrDefault();
                user.FirstName = userViewModel.FirstName;
                user.LastName = userViewModel.LastName;
                user.Email = userViewModel.Email;

                uManager.Update(user);
                db.SaveChanges();
                return RedirectToAction("TeacherIndex", "UserViewModels");
            }
            return View(userViewModel);
        }

        //Get 
        public ActionResult DeleteTeacher(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser thisUser = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (thisUser == null)
            {
                return HttpNotFound();
            }
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Id = thisUser.Id;
            userViewModel.FirstName = thisUser.FirstName;
            userViewModel.LastName = thisUser.LastName;
            userViewModel.Email = thisUser.Email;
            return View(userViewModel);
        }




        // GET: UserViewModels/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser thisUser = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (thisUser == null)
            {
                return HttpNotFound();
            }
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Id = thisUser.Id;
            userViewModel.FirstName = thisUser.FirstName;
            userViewModel.LastName = thisUser.LastName;
            userViewModel.Email = thisUser.Email;
            userViewModel.Course = thisUser.Course;
            return View(userViewModel);
        }

        // POST: UserViewModels/Delete/5
        [HttpPost, ActionName("DeleteTeacher")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteTeacherConfirmed(string id)
        {
            var uStore = new UserStore<ApplicationUser>(db);
            var uManager = new UserManager<ApplicationUser>(uStore);

            ApplicationUser user = db.Users.Where(u => u.Id == id).FirstOrDefault();
 

            uManager.Delete(user);
            db.SaveChanges();
            return RedirectToAction("TeacherIndex");
        }
        // POST: UserViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteConfirmed(string id)
        {
            var uStore = new UserStore<ApplicationUser>(db);
            var uManager = new UserManager<ApplicationUser>(uStore);

            ApplicationUser user = db.Users.Where(u => u.Id == id).FirstOrDefault();
            int courseId = user.Course.Id;

            uManager.Delete(user);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = courseId });
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
