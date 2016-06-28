namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveViewModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserViewModels", "Course_Id", "dbo.Courses");
            DropIndex("dbo.UserViewModels", new[] { "Course_Id" });
            DropTable("dbo.AddModuleViewModels");
            DropTable("dbo.AddUserViewModels");
            DropTable("dbo.UserViewModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        DefaultPassword = c.String(),
                        UserType = c.Int(nullable: false),
                        Course_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.AddModuleViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        CourseName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UserViewModels", "Course_Id");
            AddForeignKey("dbo.UserViewModels", "Course_Id", "dbo.Courses", "Id");
        }
    }
}
