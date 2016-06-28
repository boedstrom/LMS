namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddUserViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        CourseName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        DefaultPassword = c.String(),
                        UserType = c.Int(nullable: false),
                        AddUserViewModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AddUserViewModels", t => t.AddUserViewModel_Id)
                .Index(t => t.AddUserViewModel_Id);
            
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserViewModels", "AddUserViewModel_Id", "dbo.AddUserViewModels");
            DropIndex("dbo.UserViewModels", new[] { "AddUserViewModel_Id" });
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropTable("dbo.UserViewModels");
            DropTable("dbo.AddUserViewModels");
        }
    }
}
