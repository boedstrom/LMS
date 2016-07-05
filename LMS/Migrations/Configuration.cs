namespace LMS.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LMS.Models.ApplicationDbContext context)
        {
            //var rStore = new RoleStore<IdentityRole>(context);
            //var rManager = new RoleManager<IdentityRole>(rStore);
            //var role = new IdentityRole { Name = "Teacher" };
            //rManager.Create(role);

            //var uStore = new UserStore<ApplicationUser>(context);
            //var uManager = new UserManager<ApplicationUser>(uStore);
            //var user = new ApplicationUser { UserName = "admin@lexicon.se", Email = "admin@lexicon.se", FirstName = "Admin", LastName = "Lexicon" };
            //uManager.Create(user, "Password");
            //uManager.AddToRole(user.Id, role.Name);

            //user = new ApplicationUser { UserName = "teacher1@lexicon.se", Email = "teacher1@lexicon.se", FirstName = "Teacher1", LastName = "Lexicon" };
            //uManager.Create(user, "Password");
            //uManager.AddToRole(user.Id, role.Name);

            //user = new ApplicationUser { UserName = "teacher2@lexicon.se", Email = "teacher2@lexicon.se", FirstName = "Teacher2", LastName = "Lexicon" };
            //uManager.Create(user, "Password");
            //uManager.AddToRole(user.Id, role.Name);

            //user = new ApplicationUser { UserName = "teacher3@lexicon.se", Email = "teacher3@lexicon.se", FirstName = "Teacher3", LastName = "Lexicon" };
            //uManager.Create(user, "Password");
            //uManager.AddToRole(user.Id, role.Name);
            //context.SaveChanges();

            //Course[] courses = new[]
            //{
            //    new Course {Name = "Test course 1", Description = "This is Test course 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(3) },
            //    new Course {Name = "Test course 2", Description = "This is Test course 2", StartDate = DateTime.Now.AddMonths(1), EndDate = DateTime.Now.AddMonths(4) },
            //    new Course {Name = "Test course 3", Description = "This is Test course 3", StartDate = DateTime.Now.AddMonths(2), EndDate = DateTime.Now.AddMonths(5) },
            //    new Course {Name = "Test course 4", Description = "This is Test course 4", StartDate = DateTime.Now.AddMonths(3), EndDate = DateTime.Now.AddMonths(6) },
            //    new Course {Name = "Test course 5", Description = "This is Test course 5", StartDate = DateTime.Now.AddMonths(4), EndDate = DateTime.Now.AddMonths(7) }
            //};
            //context.Courses.AddOrUpdate(c => c.Name, courses);
            //context.SaveChanges();

            //Course course = context.Courses.FirstOrDefault(c => c.Id == 1);

            //Module[] modules = new[]
            //{
            //    new Module {Name = "Module 1-1", Description = "Module 1 for Test course 1", StartDate = course.StartDate, EndDate = course.StartDate.AddDays(3), Course = course },
            //    new Module {Name = "Module 1-2", Description = "Module 2 for Test course 1", StartDate = course.StartDate.AddDays(4), EndDate = course.StartDate.AddDays(6), Course = course },
            //    new Module {Name = "Module 1-3", Description = "Module 3 for Test course 1", StartDate = course.StartDate.AddDays(7), EndDate = course.StartDate.AddDays(9), Course = course },
            //    new Module {Name = "Module 1-4", Description = "Module 4 for Test course 1", StartDate = course.StartDate.AddDays(10), EndDate = course.StartDate.AddDays(12), Course = course },
            //    new Module {Name = "Module 1-5", Description = "Module 5 for Test course 1", StartDate = course.StartDate.AddDays(13), EndDate = course.StartDate.AddDays(15), Course = course }
            //};
            //context.Modules.AddOrUpdate(m => m.Name, modules);
            //context.SaveChanges();

            // rStore = new RoleStore<IdentityRole>(context);
            // rManager = new RoleManager<IdentityRole>(rStore);
            // role = new IdentityRole { Name = "Student" };
            // rManager.Create(role);

            // uStore = new UserStore<ApplicationUser>(context);
            // uManager = new UserManager<ApplicationUser>(uStore);

            // user = new ApplicationUser { UserName = "emil.kork@gmail.com", Email = "emil.kork@gmail.com", FirstName = "Emil", LastName = "Kork", Course = course };
            //uManager.Create(user, "Abcd1234");
            //uManager.AddToRole(user.Id, role.Name);

            //user = new ApplicationUser { UserName = "frollo.svedin@hotmail.com", Email = "frollo.svedin@hotmail.com", FirstName = "Frollo", LastName = "Svedin", Course = course };
            //uManager.Create(user, "Abcd1234");
            //uManager.AddToRole(user.Id, role.Name);

            //user = new ApplicationUser { UserName = "kurt.raddjur@yahoo.com", Email = "kurt.raddjur@yahoo.com", FirstName = "Kurt", LastName = "Råddjur", Course = course };
            //uManager.Create(user, "Abcd1234");
            //uManager.AddToRole(user.Id, role.Name);
            //context.SaveChanges();

            //course = context.Courses.FirstOrDefault(c => c.Id == 2);
            //modules = new[]
            //{
            //    new Module {Name = "Module 2-1", Description = "Module 1 for Test course 2", StartDate = course.StartDate, EndDate = course.StartDate.AddDays(3), Course = course },
            //    new Module {Name = "Module 2-2", Description = "Module 2 for Test course 2", StartDate = course.StartDate.AddDays(4), EndDate = course.StartDate.AddDays(6), Course = course },
            //    new Module {Name = "Module 2-3", Description = "Module 3 for Test course 2", StartDate = course.StartDate.AddDays(7), EndDate = course.StartDate.AddDays(9), Course = course },
            //    new Module {Name = "Module 2-4", Description = "Module 4 for Test course 2", StartDate = course.StartDate.AddDays(10), EndDate = course.StartDate.AddDays(12), Course = course },
            //    new Module {Name = "Module 2-5", Description = "Module 5 for Test course 2", StartDate = course.StartDate.AddDays(13), EndDate = course.StartDate.AddDays(15), Course = course }
            //};
            //context.Modules.AddOrUpdate(m => m.Name, modules);
            //context.SaveChanges();

            //user = new ApplicationUser { UserName = "user1@gmail.com", Email = "user1@gmail.com", FirstName = "First", LastName = "User", Course = course };
            //uManager.Create(user, "Abcd1234");
            //uManager.AddToRole(user.Id, role.Name);

            //user = new ApplicationUser { UserName = "user2@gmail.com", Email = "user2@gmail.com", FirstName = "Second", LastName = "User", Course = course };
            //uManager.Create(user, "Abcd1234");
            //uManager.AddToRole(user.Id, role.Name);

            //user = new ApplicationUser { UserName = "user3@gmail.com", Email = "user3@gmail.com", FirstName = "Third", LastName = "User", Course = course };
            //uManager.Create(user, "Abcd1234");
            //uManager.AddToRole(user.Id, role.Name);
            //context.SaveChanges();

            //course = context.Courses.FirstOrDefault(c => c.Id == 1);
            //Module module = context.Modules.FirstOrDefault(m => m.Course.Id == course.Id);

            //DateTime startTime = module.StartDate;
            //DateTime endTime = module.StartDate;
            //TimeSpan ts = new TimeSpan(08, 30, 0);
            //startTime = startTime.Date + ts;
            //ts = new TimeSpan(17, 00, 0);
            //endTime = endTime.Date + ts;

            //Activity[] activities = new[]
            //{
            //    new Activity {Name = "Activity 1-1-1", Description = "Activity 1 for Module 1 for Test course 1", StartTime = startTime, EndTime = endTime, Module = module },
            //    new Activity {Name = "Activity 1-1-2", Description = "Activity 2 for Module 1 for Test course 1", StartTime = startTime.AddDays(1), EndTime = endTime.AddDays(1), Module = module },
            //    new Activity {Name = "Activity 1-1-3", Description = "Activity 3 for Module 1 for Test course 1", StartTime = startTime.AddDays(2), EndTime = endTime.AddDays(2), Module = module },
            //    new Activity {Name = "Activity 1-1-4", Description = "Activity 4 for Module 1 for Test course 1", StartTime = startTime.AddDays(3), EndTime = endTime.AddDays(3), Module = module }
            //};
            //context.Activities.AddOrUpdate(a => a.Name, activities);
            //context.SaveChanges();
        }
    }
}
