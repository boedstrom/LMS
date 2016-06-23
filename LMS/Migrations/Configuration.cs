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
            //var role = new IdentityRole { Name = "teacher" };
            //rManager.Create(role);

            //var uStore = new UserStore<ApplicationUser>(context);
            //var uManager = new UserManager<ApplicationUser>(uStore);
            //var user = new ApplicationUser { UserName = "admin@lexicon.se", Email = "admin@lexicon.se" };
            //uManager.Create(user, "password");

            //uManager.AddToRole(user.Id, role.Name);
        }
    }
}
